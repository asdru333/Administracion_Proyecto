function validarInfoPersonal() {
    document.getElementById("formularioDatosPersonales").style.display = "none";
    document.getElementById("formularioDatosPago").style.display = "contents";
}

$(document).ready(function() {
  $(window).keydown(function(event){
    if(event.keyCode == 13) {
      event.preventDefault();  
      return false;
    }
  });
});