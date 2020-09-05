<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Locate_User.aspx.cs" Inherits="OpenOrderFramework.Location" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>Get User Current Location using Google Map Geolocation API Service in asp.net website</title>
<style type="text/css">
html { height: 100% }
body { height: 100%; margin: 0; padding: 0 }
#map_canvas { height: 100% }
</style>
<script type="text/javascript"
src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDOuuRmnS__GLIY_FetpSxVN9y_kdASjMM&sensor=false">
</script>
<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false&libraries=places">
</script>
<script type="text/javascript">
if (navigator.geolocation) {
navigator.geolocation.getCurrentPosition(success);
} else {
alert("Geo Location is not supported on your current browser!");
}
function success(position) {
var lat = position.coords.latitude;
var long = position.coords.longitude;
var city=position.coords.locality;
var myLatlng = new google.maps.LatLng(lat, long);
var myOptions = {
center: myLatlng,
zoom: 12,
mapTypeId: google.maps.MapTypeId.ROADMAP
};
var map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
var marker = new google.maps.Marker({
position: myLatlng,
title: "lat: " + lat + " long: " + long
});


 
marker.setMap(map);
var infowindow = new google.maps.InfoWindow({ content: "<b>User Address</b><br/> Latitude:"+lat+"<br /> Longitude:"+long+"" });
infowindow.open(map, marker);
}
</script>
</head>
<body >
<form id="form1" runat="server">
<div id="map_canvas" style="width: 500px; height: 400px"></div>
</form>
</body>
</html>
