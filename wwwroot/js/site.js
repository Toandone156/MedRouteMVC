// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    var elementToScrollTo = document.getElementById("container");

    if (elementToScrollTo) {
        elementToScrollTo.scrollIntoView();
    }
});

// send request [post] book
        function bookRequestPost(bookingIDStr) {
            var fd = new FormData();
            fd.append('bookingIDStr', bookingIDStr);

            $.ajax({
                url: "/Booking/Book",
                type: "POST",
                data: fd,
                processData: false,  // Không xử lý dữ liệu form
                contentType: false,  // Không thiết lập contentType, để trình duyệt tự động xác định
                cache: false,
                success: function (data) {
                    //alert(data);
                    window.location.reload();
                },
                error: function (xhr) {
                    //alert('error');
                }
            });
        }
// askForBooking
function askForBookingModal(medicalRecord) {
    if (medicalRecord == undefined || medicalRecord == null) {
        var modal = document.getElementById("askForBooking");
        modal.style.display = "block";
    }
}
/*function askForBookingModal() {
    var modal = document.getElementById("askForBooking");
    modal.style.display = "block";
}*/