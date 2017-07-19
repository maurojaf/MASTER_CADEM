

function validarSiNumero(control, numero) {
    if (!/^([0-9])*$/.test(numero)) {
        control.style.backgroundColor = "#f7eecd";
        control.value = "";
    } else {
        control.style.backgroundColor = "white";
    }
}

function PintaSiVacio(control, Valor) {
    Valor = Valor.replace(/([\ \t]+(?=[\ \t])|^\s+|\s+$)/g, '');
    if (Valor != "") {
        control.value = Valor.toUpperCase();
        control.style.backgroundColor = "white";
    }
}

function RestarHoras(inicio, fin) {

    var inicioMinutos = parseInt(inicio.substr(3, 2));
    var inicioHoras = parseInt(inicio.substr(0, 2));

    var finMinutos = parseInt(fin.substr(3, 2));
    var finHoras = parseInt(fin.substr(0, 2));

    var transcurridoMinutos = finMinutos - inicioMinutos;
    var transcurridoHoras = finHoras - inicioHoras;

    if (transcurridoMinutos < 0) {
        transcurridoHoras--;
        transcurridoMinutos = 60 + transcurridoMinutos;
    }

    var horas = transcurridoHoras.toString();
    var minutos = transcurridoMinutos.toString();

    return horas + " Horas, " + minutos + " Min.";
}

function validaemail(Control, Valor) {
    var expr = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (!expr.test(Valor)) {
        Control.style.backgroundColor = "#f7eecd";
        Control.value = "";
    } else {
        Control.style.backgroundColor = "white";
    }
}