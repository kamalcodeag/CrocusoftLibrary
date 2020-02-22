$(document).ready(function () {
    "use strict"; $(document).on("click", ".sub-menu", function () {
        $(this).children(".sub").toggle(); $(this).children(".sub").toggleClass("forScroll"); if ($(this).children(".sub").hasClass("forScroll")) { $("#sidebar").css({ "overflow-y": "scroll" }) }
        else { $("#sidebar").css({ "overflow-y": "auto" }) }
    })
    $("footer .fa-angle-up").click(function () { $("html").animate({ scrollTop: 0 }, 500) })
})