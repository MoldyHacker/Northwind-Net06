$(function () {
    getProducts()
    
    function getProducts() {
      var discontinued = $('#Discontinued').prop('checked') ? "" : "/discontinued/false";
      $.getJSON({
        url: `../../api/category/${$('#product_rows').data('id')}/productwithreviews` + discontinued,
        success: function (response, textStatus, jqXhr) {
          $('#product_rows').html("");
            for (var i = 0; i < response.length; i++){
              var css = response[i].discontinued ? " class='discontinued'" : "";
              var rating = Math.round(response[i].avgRating);
              var row = `<tr${css} data-id="${response[i].productId}" data-name="${response[i].productName}" data-price="${response[i].unitPrice}">
                <td>${response[i].productName}</td>
                <td class="text-right">${response[i].unitPrice.toFixed(2)}</td>
                <td class="text-right">${response[i].unitsInStock}</td>
                <td class="text-right rating" data-id="${response[i].productId}">${getStars(rating)}</td>
                </tr>`;
              $('#product_rows').append(row);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
          // log the error to the console
          console.log("The following error occured: " + textStatus, errorThrown);
        }
      });
    }
    
    $('#CategoryId').on('change', function(){
      $('#product_rows').data('id', $(this).val());
      getProducts();
    });

    $('#Discontinued').on('change', function(){
      getProducts();
    });

    // delegated event listener
    $('#product_rows').on('click', 'tr', function(){
      $('.rating').on('click', function(){
        
        window.location.href = '/product/reviews/' + $(this).data('id');
      })
        // make sure a customer is logged in
      if ($('#User').data('customer').toLowerCase() == "true"){
        $('#ProductId').html($(this).data('id'));
        $('#ProductName').html($(this).data('name'));
        $('#UnitPrice').html($(this).data('price').toFixed(2));
        // calculate and display total in modal
        $('#Quantity').change();
        $('#cartModal').modal();
      } else {
        toast("Access Denied", "You must be signed in as a customer to access the cart.");
      }
    });

      // update total when cart quantity is changed
    $('#Quantity').change(function () {
        var total = parseInt($(this).val()) * parseFloat($('#UnitPrice').html());
        $('#Total').html(numberWithCommas(total.toFixed(2)));
    });
    
      // function to display commas in number
    function numberWithCommas(x) {
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
    $('#addToCart').on('click', function(){
        $('#cartModal').modal('hide');
        $.ajax({
          headers: { "Content-Type": "application/json" },
          url: "../../api/addtocart",
          type: 'post',
          data: JSON.stringify({
            "id": Number($('#ProductId').html()),
            "email": $('#User').data('email'),
            "qty": Number($('#Quantity').val()) 
          }),
          success: function (response, textStatus, jqXhr) {
            // success
            toast("Product Added", `${response.product.productName} successfully added to cart.`);
          },
          error: function (jqXHR, textStatus, errorThrown) {
            // log the error to the console
            console.log("The following error occured: " + jqXHR.status, errorThrown);
            toast("Error", "Please try again later.");
          }
        });
    });
    function toast(header, message) {
        $('#toast_header').html(header);
        $('#toast_body').html(message);
        $('#cart_toast').toast({ delay: 2500 }).toast('show');
      }
    function getStars(rating){
      switch(rating){
        case 1:
          return '<i class="fa fa-star checked"></i><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i>';
          break;
        case 2:
          return '<i class="fa fa-star checked"></i><i class="fa fa-star checked"></i><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i>';
          break;
        case 3:
          return '<i class="fa fa-star checked"></i><i class="fa fa-star checked"></i><i class="fa fa-star checked"></i><i class="fa fa-star"></i><i class="fa fa-star"></i>';
          break;
        case 4:
          return '<i class="fa fa-star checked"></i><i class="fa fa-star checked"></i><i class="fa fa-star checked"></i><i class="fa fa-star checked"></i><i class="fa fa-star"></i>';
          break;
        case 5:
          return '<i class="fa fa-star checked"></i><i class="fa fa-star checked"></i><i class="fa fa-star checked"></i><i class="fa fa-star checked"></i><i class="fa fa-star checked"></i>';
          break;
        default:
          return '<i class="fa fa-star checked"></i><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i>';
          break;

        }
       
    }
    });