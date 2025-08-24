window.onload = function () {
    const libri = document.getElementById("libri");
    const idCliente = document.getElementById("idCliente").value;
    const dataInizio = document.getElementById("dataInizio");
    const dataFine = document.getElementById("dataFine");

    const flatDataInizio = flatpickr("#dataInizio", {
        minDate: "today",
        locale: "it",
        altInput: true,
        altFormat: "d/m/Y",
        dateFormat: "Y-m-d",
        altInputClass: "form-control",
        onChange: function (selectedDates, dateStr, instance) {
            document.querySelector("#dataInizioHidden").value = dateStr; // aggiorna hidden
        }
    });
    setDateBloccate(libri.value, idCliente);

    libri.addEventListener("change", function () {
        setDateBloccate(this.value, idCliente);
    });

    const flatDataFine = flatpickr("#dataFine", {
        locale: "it",
        clickOpens: false,
        altInput: true,
        altFormat: "d/m/Y",
        dateFormat: "Y-m-d",
        altInputClass: "form-control",
        onChange: function (selectedDates, dateStr, instance) {
            document.querySelector("#dataFineHidden").value = dateStr; // aggiorna hidden
        }
    });

    dataInizio.addEventListener("change", function () {
        flatDataFine.set("minDate", this.value);
        flatDataFine.set("clickOpens", true);
        flatDataFine.open();
        flatDataFine.altInput.disabled = false;
    });
}

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

            if(dateBloccate.length > 0)
            {
                const arrayDateBloccateFlat = [];

                for (const data of dateBloccate) {
                    arrayDateBloccateFlat.push({
                        from: data.dataInizio,
                        to: data.dataFine
                    });
                }

                flatDataInizio.set("disable", arrayDateBloccateFlat);
                flatDataFine.set("disable", arrayDateBloccateFlat);
            })
        })
        .catch(error => {
            console.error("Errore fetch:", error);
        });
}