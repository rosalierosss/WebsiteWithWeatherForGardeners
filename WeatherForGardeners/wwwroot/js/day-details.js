let pendingChanges = { added: [], edited: [], updatedStatuses: [] };

// Обновление статуса задачи
document.querySelectorAll(".task-status").forEach((checkbox) => {
    checkbox.addEventListener("change", function () {
        const taskId = parseInt(this.dataset.taskId, 10);
        const isCompleted = this.checked;

        pendingChanges.updatedStatuses.push({ taskId, isCompleted });
    });
});

// Редактирование задачи
document.querySelectorAll(".edit-button").forEach((button) => {
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

// Добавление задачи
document.getElementById("add-task-button").addEventListener("click", function () {
    document.getElementById("modal-title").innerText = "Добавить задачу";
    document.getElementById("edit-task-id").value = "";
    document.getElementById("edit-task-title").value = "";
    document.getElementById("edit-task-description").value = "";

    document.getElementById("edit-modal").style.display = "block";
});

// Отмена редактирования
document.getElementById("cancel-edit").addEventListener("click", function () {
    document.getElementById("edit-modal").style.display = "none";
});

// Обработка сохранения формы (добавление или редактирование)
document.getElementById("edit-form").addEventListener("submit", function (e) {
    e.preventDefault();

    const taskId = document.getElementById("edit-task-id").value;
    const taskTitle = document.getElementById("edit-task-title").value;
    const taskDescription = document.getElementById("edit-task-description").value;

    if (taskId) {
        // Редактирование задачи
        pendingChanges.edited.push({
            taskId: parseInt(taskId, 10),
            title: taskTitle,
            description: taskDescription,
        });

        const taskElement = document.querySelector(`[data-task-id='${taskId}']`).closest("li");
        taskElement.querySelector("strong").innerText = taskTitle;
        taskElement.querySelector("p").innerText = taskDescription;
    } else {
        // Добавление новой задачи
        const tempId = Date.now(); // Генерация временного ID
        pendingChanges.added.push({ tempId, title: taskTitle, description: taskDescription });

        // Отображение новой задачи
        const newTask = document.createElement("li");
        newTask.innerHTML = `
            <strong>${taskTitle}</strong>
            <p>${taskDescription}</p>
            <label>
                <input type="checkbox" class="task-status" data-task-id="${tempId}" />
                Выполнено
            </label>
            <button class="edit-button" data-task-id="${tempId}" data-task-title="${taskTitle}" data-task-description="${taskDescription}">Редактировать</button>
        `;
        document.getElementById("task-list").appendChild(newTask);
    }

    document.getElementById("edit-modal").style.display = "none";
});

// Сохранение изменений
document.getElementById("save-button").addEventListener("click", function () {
    // Используем глобальную переменную currentDay
    fetch(`/DayDetails/SaveAllChanges?date=${encodeURIComponent(currentDay)}`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(pendingChanges),
    })
        .then((response) => {
            if (!response.ok) {
                throw new Error(`HTTP Error: ${response.status}`);
            }
            return response.json();
        })
        .then((data) => {
            if (data.success) {
                showNotification("Все изменения сохранены");

                // Очистить локальные изменения
                pendingChanges = { added: [], edited: [], updatedStatuses: [] };

                console.log("Изменения сохранены на сервере.");
            } else {
                showNotification("Ошибка при сохранении", true);
            }
        })
        .catch((error) => {
            console.error("Ошибка соединения с сервером:", error);
            showNotification("Ошибка соединения", true);
        });
});



// Функция отображения уведомлений
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
