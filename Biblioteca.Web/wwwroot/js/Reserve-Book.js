const dataInizio = document.getElementById("dataInizio");
const dataFine = document.getElementById("dataFine");

dataInizio.addEventListener("input", function () {
    dataFine.disabled = false;
    dataFine.min = dataInizio.value;
});