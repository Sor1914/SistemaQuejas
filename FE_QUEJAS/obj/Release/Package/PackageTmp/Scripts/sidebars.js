/* global bootstrap: false */
(function () {
  'use strict'
  var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
  tooltipTriggerList.forEach(function (tooltipTriggerEl) {
    new bootstrap.Tooltip(tooltipTriggerEl)
  })
})()

//Ocultar y mostrar sidebar
var toggleButton = document.querySelector('[data-toggle="sidebarMenu"]');
var menu = document.getElementById('sidebarMenu');
var icono = document.getElementById('iconoBtnSidebar');
const contenedor = document.getElementById("cuerpoPagina");
const contenedorSidebar = document.getElementById("contenedorSidebar");


toggleButton.addEventListener('click', function () {
    if (menu.classList.contains('d-none')) {
        menu.classList.remove('d-none');
        contenedorSidebar.appendChild(toggleButton);
        icono.classList.remove('fa-bars');
        icono.classList.add('fa-minus');
        toggleButton.classList.remove('btn-outline-dark');
        toggleButton.classList.add('btn-outline-light');
        toggleButton.classList.remove('btnMenuCuerpo');
    } else {
        menu.classList.add('d-none');     
        contenedor.appendChild(toggleButton);
        icono.classList.remove('fa-minus');
        icono.classList.add('fa-bars');                
        toggleButton.classList.remove('btn-outline-light');
        toggleButton.classList.add('btn-outline-dark');
        toggleButton.classList.add('btnMenuCuerpo');
    
    }
});
