$(function (){
    $('#inputReview').on('click', function(){
        
        $.ajax({
          headers: { "Content-Type": "application/json" },
          url: "../../api/inputreview",
          type: 'post',
          data: JSON.stringify({
            "productId": Number($('#ProductId').html()),
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
})