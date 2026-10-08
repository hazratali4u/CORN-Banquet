var CORN = CORN || {};

CORN.MenuCard = {
    handlerURL: "../Handlers/MenuCardWS.asmx",
    optionUITemplate: {},
    MenuItemTemplate: {},

    CategoryList: [],
    SKUInfoList: [],
    SelectedCategory: null,

    SKUInfoListByCategory: [],

    getData: function () {

        blockUI();
        $.ajax({
            cache: false,
            async: true,
            dataType: "json",
            type: "POST",
            url: CORN.MenuCard.handlerURL + '/GetMenuCardDetails',
            success: function (response) {
                switch (response.Status) {
                    case "Success":
                        unblockUI();

                        CORN.MenuCard.CategoryList = response.Records.CategoryList;
                        CORN.MenuCard.SKUInfoList = response.Records.SKUInfoList;
                        CORN.MenuCard.populateCategoryList();

                        break;
                    case "Error":
                        unblockUI();
                        showMessageDialog(response.Message, 'Error');
                        break;
                }
            }
        });
    },

    populateCategoryList: function () {
        var ddl = $('#ddlCategory');
        $(ddl).find("option:gt(0)").remove();

        var firstRecord = null;
        $(CORN.MenuCard.CategoryList).each(function (index, data) {
            var optionItem = CORN.MenuCard.optionUITemplate.clone().attr("value", data.CategoryId).text(data.CategoryName);
            $(ddl).append(optionItem);
            if (index == 0)
                firstRecord = data;
        });
        if (firstRecord != null) {
            $(ddl).find("option[value='" + firstRecord.CategoryId + "']").prop('selected', true);
        }

        //$(ddl).trigger('chosen:updated');
        CORN.MenuCard.SelectedCategory = firstRecord;
        CORN.MenuCard.filterSKUsByCategoryId();
    },
    populateCategoryListNew: function () {
        var firstRecord = null;
        ddlCategory.ClearItems();
        $(CORN.MenuCard.CategoryList).each(function (index, data) {
            ddlCategory.AddItem(data.CategoryName, data.CategoryId);
            if (index == 0)
                firstRecord = data;
        });
        if (firstRecord != null) {
            CORN.MenuCard.SelectedCategory = firstRecord;
            ddlCategory.SetValue(firstRecord.CategoryId);
        }

        CORN.MenuCard.filterSKUsByCategoryId();
    },

    filterSKUsByCategoryId: function () {
        if (CORN.MenuCard.SelectedCategory != null) {

            CORN.MenuCard.SKUInfoListByCategory = CORN.MenuCard.SKUInfoList.filter(function (element) {
                return (element.CATEGORY_ID == CORN.MenuCard.SelectedCategory.CategoryId)
            });
            if (CORN.MenuCard.SKUInfoListByCategory.length == 0) {
                showMessageDialog("No item found in category '" + CORN.MenuCard.SelectedCategory.CategoryName + "'", 'No Record Found');
            }
            else {
                CORN.MenuCard.populateMenuItems();
            }
        }
        else {
            showMessageDialog("Please select category", 'Error');
        }
    },

    populateMenuItemsOld: function () {
        $('#menu-container').html('');
        var $template = null;
        var $rowDiv = $('<div>', {
            'class': 'row'
        });

        var leftPaneSKUList = [],
            rightPaneSKUList = [];

        var half = parseInt(CORN.MenuCard.SKUInfoListByCategory.length / 2);
        half = half + (CORN.MenuCard.SKUInfoListByCategory.length % 2);        

        $.each(CORN.MenuCard.SKUInfoListByCategory, function (index, data) {
            if (half > index) {
                leftPaneSKUList.push(data);
            }
            else {
                rightPaneSKUList.push(data);
            }
        });

        var $leftPaneSKUDiv = $('<div>', {
            'class': 'col-lg-6 col-md-6 col-sm-6'
        }).appendTo($rowDiv);
        $.each(leftPaneSKUList, function (index, data) {
            $template = $(CORN.MenuCard.MenuItemTemplate).clone();

            $img = $template.find('.menu-image');
            $img.attr('src',
                function () {
                    var path = '';
                    if (data.SKU_IMAGE.length == 0)
                        path = '../images/no_image.gif';
                    else
                        path = '../SkuImages/' + data.SKU_IMAGE;
                    return path;
                });
            $img.data('record', data);
            $img.on('click', function () {
                $('.imagepreview').attr('src', $(this).attr('src'));
                $('#imagemodal').find('.modal-title').html($(this).data('record').SKU_NAME);
                $('#imagemodal').modal('show');
            })
            .css({
                'cursor': 'pointer'
            });
            $template.find('.menu-header').html(data.SKU_NAME);
            $template.find('.menu-price').html('Rs. ' + data.PRICE);
            $template.find('.menu-description').html('<p>' + data.DESCRIPTION + '</p>');

            $leftPaneSKUDiv.append($template);
        });

        var $rightPaneSKUDiv = $('<div>', {
            'class': 'col-lg-6 col-md-6 col-sm-6'
        }).appendTo($rowDiv);
        $.each(rightPaneSKUList, function (index, data) {
            $template = $(CORN.MenuCard.MenuItemTemplate).clone();

            $img = $template.find('.menu-image');
            $img.attr('src',
                function () {
                    var path = '';
                    if (data.SKU_IMAGE.length == 0)
                        path = '../images/no_image.gif';
                    else
                        path = '../SkuImages/' + data.SKU_IMAGE;
                    return path;
                });
            $img.on('click', function () {
                $('.imagepreview').attr('src', $(this).attr('src'));
                $('#imagemodal').modal('show');
            })
            .css({
                'cursor': 'pointer'
            });
            $template.find('.menu-header').html(data.SKU_NAME);
            $template.find('.menu-price').html('Rs. ' + data.PRICE);
            $template.find('.menu-description').html('<p>' + data.DESCRIPTION + '</p>');

            $rightPaneSKUDiv.append($template);
        });

        $('#menu-container').append($rowDiv);
    },

    populateMenuItems: function () {
        $('#menu-container').html('');
        var $template = null;

        $.each(CORN.MenuCard.SKUInfoListByCategory, function (index, data) {
            $template = $(CORN.MenuCard.MenuItemTemplate).clone();

            $img = $template.find('.menu-image');
            $img.attr('src',
                function () {
                    var path = '';
                    if (data.SKU_IMAGE.length == 0)
                        path = '../images/no_image.gif';
                    else
                        path = '../SkuImages/' + data.SKU_IMAGE;
                    return path;
                });
            $img.data('record', data);
            $img.on('click', function () {
                $('.imagepreview').attr('src', $(this).attr('src'));
                $('#imagemodal').find('.modal-title').html($(this).data('record').SKU_NAME);
                $('#imagemodal').modal('show');
            })
            .css({
                'cursor': 'pointer'
            });
            $template.find('.menu-header').html(data.SKU_NAME);
            $template.find('.menu-price').html('Rs. ' + data.PRICE);
            $template.find('.menu-description').html('<p>' + data.DESCRIPTION + '</p>');

            $('#menu-container').append($template);
        });
    },

    OnCategoryChanged: function (s, e) {

        var categoryId = s.GetValue();
        var result = CORN.MenuCard.CategoryList.filter(function (element) {
            return (element.CategoryId == parseInt(categoryId))
        });
        if (result.length > 0)
            CORN.MenuCard.SelectedCategory = result[0];
        else
            CORN.MenuCard.SelectedCategory = null;

        CORN.MenuCard.filterSKUsByCategoryId();

    }
};


$(function () {

    //$('#ddlCategory').chosen();

    CORN.MenuCard.MenuItemTemplate = $(".MenuItemTemplate").clone().removeClass('MenuItemTemplate');
    CORN.MenuCard.optionUITemplate = $("option", ".ddlTemplate").clone();

    $(document).on("change", "#ddlCategory", function (e) {
        var categoryId = $(e.currentTarget).val();
        var result = CORN.MenuCard.CategoryList.filter(function (element) {
            return (element.CategoryId ==  parseInt(categoryId))
        });
        if (result.length > 0)
            CORN.MenuCard.SelectedCategory = result[0];
        else
            CORN.MenuCard.SelectedCategory = null;

        CORN.MenuCard.filterSKUsByCategoryId();
    });
    CORN.MenuCard.getData();

});