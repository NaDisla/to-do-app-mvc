
let btnAdd = document.querySelector("#btn-add");
let task = document.querySelector("#task");
let msgValidation = document.querySelector("#msg-validation");
let btnDone = document.querySelectorAll("#btn-done");
let btnEdit = document.querySelectorAll("#btn-edit");
let btnDelete = document.querySelectorAll("#btn-delete");
let btnClear = document.querySelector("#btn-clear");
let taskName = document.querySelectorAll("#task-name");
let iconDoneUndone = document.querySelectorAll("#icon-done-undone");
let idTask = document.querySelectorAll("#id-task");

for (let i = 0; i < btnDone.length; i++) {
    btnDone[i].addEventListener('click', () => {
        if (taskName[i].classList.contains('text-decoration-line-through')) {
            taskName[i].classList.remove('text-decoration-line-through');
            iconDoneUndone[i].classList.remove('fa-circle-minus');
            iconDoneUndone[i].classList.add('fa-circle-check')
        }
        else {
            taskName[i].classList.add('text-decoration-line-through');
            iconDoneUndone[i].classList.remove('fa-circle-check');
            iconDoneUndone[i].classList.add('fa-circle-minus')
        }
    });
}

for (let i = 0; i < btnEdit.length; i++) {
    btnEdit[i].addEventListener('click', () => {
        let idTaskGet = $(idTask[i]).text();
        $.ajax({
            url: 'Home/PopulateForm',
            type: 'GET',
            data: {
                id: idTaskGet
            },
            dataType: 'json',
            success: (res) => {
                $("#task-id").val(res.id);
                $("#task").val(res.name);
                $("#form-action").attr("action", "/Home/Update");
            }
        });
    })
}

for (let i = 0; i < btnEdit.length; i++) {
    btnDelete[i].addEventListener('click', () => {
        let idTaskGet = $(idTask[i]).text();
        $.ajax({
            url: 'Home/Delete',
            type: 'POST',
            data: {
                id: idTaskGet
            },
            success: () => {
                window.location.reload();
            }
        });
    })
}

btnClear.addEventListener('click', () => {
    $.ajax({
        url: 'Home/Clear',
        type: 'POST',
        success: () => { window.location.reload(); }
    });
});