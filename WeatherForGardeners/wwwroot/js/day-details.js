document.getElementById("save-button").addEventListener("click", function () {
    const checkboxes = document.querySelectorAll(".task-status");
    const updates = [];

    checkboxes.forEach((checkbox) => {
        updates.push({
            taskId: parseInt(checkbox.dataset.taskId, 10),
            isCompleted: checkbox.checked,
        });
    });

    fetch("/DayDetails/UpdateTaskStatus", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(updates),
    })
    .then((response) => {
        if (!response.ok) {
            throw new Error(`HTTP Error: ${response.status}`);
        }
        return response.json();
    })
    .then((data) => {
        if (data.success) {
            showNotification("Изменения сохранены");
        } else {
            showNotification("Ошибка при сохранении", true);
        }
    })
    .catch((error) => {
        console.error("Ошибка соединения с сервером:", error);
        showNotification("Ошибка соединения", true);
    });
});

document.querySelectorAll(".edit-button").forEach(button => {
    button.addEventListener("click", function () {
        const taskId = this.dataset.taskId;
        const taskTitle = this.dataset.taskTitle;
        const taskDescription = this.dataset.taskDescription;

        document.getElementById("modal-title").innerText = "Редактировать задачу";
        document.getElementById("edit-task-id").value = taskId;
        document.getElementById("edit-task-title").value = taskTitle;
        document.getElementById("edit-task-description").value = taskDescription;

        document.getElementById("edit-modal").style.display = "block";
    });
});

document.getElementById("add-task-button").addEventListener("click", function () {
    document.getElementById("modal-title").innerText = "Добавить задачу";
    document.getElementById("edit-task-id").value = "";
    document.getElementById("edit-task-title").value = "";
    document.getElementById("edit-task-description").value = "";

    document.getElementById("edit-modal").style.display = "block";
});

document.getElementById("cancel-edit").addEventListener("click", function () {
    document.getElementById("edit-modal").style.display = "none";
});

document.getElementById("edit-form").addEventListener("submit", function (e) {
    e.preventDefault();

    const taskId = document.getElementById("edit-task-id").value;
    const taskTitle = document.getElementById("edit-task-title").value;
    const taskDescription = document.getElementById("edit-task-description").value;

    const url = taskId ? "/DayDetails/EditTask" : "/DayDetails/AddTask";
    const body = taskId
        ? { taskId: parseInt(taskId, 10), title: taskTitle, description: taskDescription }
        : { title: taskTitle, description: taskDescription };

    fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(body)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`Ошибка HTTP: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                showNotification(taskId ? "Задача обновлена" : "Задача добавлена");
                location.reload();
            } else {
                showNotification("Ошибка сохранения", "error");
            }
        })
        .catch(error => {
            console.error("Ошибка:", error);
            showNotification("Ошибка соединения с сервером", "error");
        });
});


function showNotification(message, isError = false) {
    const notification = document.createElement("div");
    notification.className = "notification" + (isError ? " error" : "");
    notification.textContent = message;
    document.body.appendChild(notification);

    // Показать уведомление
    setTimeout(() => {
        notification.classList.add("show");
    }, 10);

    // Удалить уведомление через 2 секунды
    setTimeout(() => {
        notification.classList.remove("show");
        setTimeout(() => notification.remove(), 300); // Удалить после исчезновения
    }, 2000);
}

/* Управление модальным окном */
function showModal(modalId) {
    document.querySelector(".modal-backdrop").style.display = "block";
    document.getElementById(modalId).style.display = "block";
}

function hideModal(modalId) {
    document.querySelector(".modal-backdrop").style.display = "none";
    document.getElementById(modalId).style.display = "none";
}
