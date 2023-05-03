$(function (){
    getPurchases()
    function getPurchases(){
        $.getJSON({
            success:function(response,textStatus,jqXhr){
                $('purchase_rows').html("");
                for(var i = 0; i<response.length; i++){
                    var row = `<tr data-id="${response[i].productId}" data-name="${response[i].productName}"></tr>`
                    $('#purchase_rows').append(row);
                }
            }
            
        })
    }
})