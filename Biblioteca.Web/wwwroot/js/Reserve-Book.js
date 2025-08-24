const libri = document.getElementById("libri");
const idCliente = document.getElementById("idCliente").value;
const dataInizio = document.getElementById("dataInizio");
const dataFine = document.getElementById("dataFine");

const flatDataInizio = flatpickr("#dataInizio", {
    minDate: "today",
    dateFormat: "d/m/Y",
    locale: "it"
});
setDateBloccate(libri.value, idCliente);

libri.addEventListener("input", function () {
    setDateBloccate(this.value, idCliente);
});

const flatDataFine = flatpickr("#dataFine", {
    dateFormat: "d/m/Y",
    locale: "it",
    clickOpens: false
});

dataInizio.addEventListener("input", function () {
    flatDataFine.set("minDate", this.value);
    dataFine.disabled = false;
    flatDataFine.open();
});

function setDateBloccate(idLibro, idCliente)
{
    fetch(`/Libro/GetDateBloccate?idLibro=${idLibro}&idCliente=${idCliente}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Errore nella risposta HTTP: " + response.status);
            }
            return response.json(); // converto in JSON
        })
        .then(data => {
            console.log("Date bloccate:", data);

            const dateBloccate = data;

            const arrayDateBloccateFlat = [];

            for(const data of dateBloccate)
            {
                arrayDateBloccateFlat.push({
                    from: data.dataInizio,
                    to: data.dataFine
                });
            }

            flatDataInizio.set("disable", arrayDateBloccateFlat);
            flatDataFine.set("disable", arrayDateBloccateFlat);

        })
        .catch(error => {
            console.error("Errore fetch:", error);
        });
}