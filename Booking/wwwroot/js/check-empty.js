function checkEmpty(items) {
    let eblock = $('#empty_list');
    if (items.length == 0 && eblock.hasClass('collapse')) {
        eblock.removeClass('collapse');
    }
    else if (items.length > 0 && !eblock.hasClass('collapse')) {
        eblock.addClass('collapse');
    }
}