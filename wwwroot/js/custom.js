// to get current year
function getYear() {
    var currentDate = new Date();
    var currentYear = currentDate.getFullYear();
    document.querySelector("#displayYear").innerHTML = currentYear;
}

getYear();


// client section owl carousel
$(".client_owl-carousel").owlCarousel({
    loop: true,
    margin: 0,
    dots: false,
    nav: true,
    navText: [],
    autoplay: true,
    autoplayHoverPause: true,
    navText: [
        '<i class="fa fa-angle-left" aria-hidden="true"></i>',
        '<i class="fa fa-angle-right" aria-hidden="true"></i>'
    ],
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 1
        },
        1000: {
            items: 2
        }
    }
});



/** google_map js **/
function myMap() {
    var mapProp = {
        center: new google.maps.LatLng(40.712775, -74.005973),
        zoom: 18,
    };
    var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
}

/*for(var el of document.getElementsByClassName("submit")){
    el.addEventListener("click", function() {
        var modal = document.getElementById("myModal");
        modal.style.display = "block"; *//* Show the modal *//*
      });
}
*/
// document.getElementsByClassName("submit").addEventListener("click", function() {
//     var modal = document.getElementById("myModal");
//     modal.style.display = "block"; /* Show the modal */
//   });

// add event close button
var closeButtonElements = document.getElementsByClassName("closeButton");

for (var i = 0; i < closeButtonElements.length; i++) {
    closeButtonElements[i].addEventListener("click", function () {
        var modals = document.getElementsByClassName("modal");

        for (var j = 0; j < modals.length; j++) {
            modals[j].style.display = "none";
        }
    });
}
