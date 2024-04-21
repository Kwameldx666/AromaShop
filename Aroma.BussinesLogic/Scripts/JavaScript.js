<><script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script><script>
    $(document).ready(function() {}
    var index = 0;

    $('#add-product-row').click(function() {}
    var newRow = $('#product-template').html();
    $('#products-container').append(newRow);
    index++;
    updateNames();
    });

    $(document).on('click', '.remove-product-row', function() {$(this).closest('.product-row').remove()};
    updateNames();
    });

    function updateNames() {$('#products-container .product-row').each(function(i) {
        $(this).find('.productName').attr('name', 'products[' + i + '].Name');
        $(this).find('.productPrice').attr('name', 'products[' + i + '].Price');
        $(this).find('.productDescription').attr('name', 'products[' + i + '].Description');
    })};
    }
    });
</script></>
