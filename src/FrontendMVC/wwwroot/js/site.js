﻿// Initialize Material Design.
$.material.init();

// Setup jQuery Validate for Bootstrap integration.
jQuery.validator.setDefaults({
    highlight: function (element) {
        jQuery(element).closest('.form-group').addClass('has-error');
    },
    unhighlight: function (element) {
        jQuery(element).closest('.form-group').removeClass('has-error');
    },
    errorElement: 'span',
    errorClass: 'label label-danger',
    errorPlacement: function (error, element) {
        if (element.parent('.input-group').length) {
            error.insertAfter(element.parent());
        } else {
            error.insertAfter(element);
        }
    }
});
