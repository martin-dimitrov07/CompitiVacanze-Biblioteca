const deleteModal = document.getElementById('deleteModal');

deleteModal.addEventListener('show.bs.modal', function (event) {
    var button = event.relatedTarget;
    var idLibro = button.getAttribute('data-id');

    var form = deleteModal.querySelector('#deleteForm');
    form.action = '/Libro/Delete/' + idLibro; // rotta Delete nel controller
});
