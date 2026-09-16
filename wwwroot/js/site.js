let palabra = document.getElementById("palabra").value.toUpperCase();
let palabraOculta = [];
let intentos = 6;

for(let i = 0; i < palabra.length; i++){
    if (palabra[i] === " ") {
        palabraOculta[i] = " ";
    } else {
        palabraOculta[i] = "_";
    }
}

document.getElementById("palabraOculta").innerHTML = palabraOculta.join(" ");

function ArriesgarLetra()
{
    let letra = document.getElementById("letra").value.toUpperCase();
    let encontrada = false;

    if (letra === "") {
        alert("Ingrese una letra");
        return;
    }

    for (let i = 0; i < palabra.length; i++) {
        if (palabra[i] === letra) {
            palabraOculta[i] = letra;
            encontrada = true;
        }
    }

    if (!encontrada && intentos > 0) {
        intentos--;
        document.getElementById("intentos").innerHTML = intentos;
    }

     document.getElementById("palabraOculta").innerHTML = palabraOculta.join(" ");
    document.getElementById("letra").value = "";

    if (!palabraOculta.includes("_")) {
        document.getElementById("mensaje").innerHTML = "¡GANASTE!";
        EliminarBoton();
        setTimeout(function(){
        window.location.href = "/Juego/Semis";
        }, 1000);
    }

    if(intentos <= 0){
        document.getElementById("mensaje").innerHTML = "PERDISTE. La palabra era: " + palabra;
        
        EliminarBoton();
         setTimeout(function(){
        window.location.href = "/Juego/Index";
        }, 1000);
    }
}

function EliminarBoton(){
    document.getElementById("boton").innerHTML = " ";
    document.getElementById("boton").style.padding = "0%";
    document.getElementById("boton").style.margin = "0%";
}