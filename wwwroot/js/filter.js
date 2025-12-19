@* JS * @
< script >
    document.addEventListener("DOMContentLoaded", () => {
        // Находим все кнопки Cancel
        document.querySelectorAll(".cancel-btn").forEach(btn => {
            btn.addEventListener("click", () => {
                // Находим контейнер биржи
                const container = btn.closest(".exchange-select-card");

                if (!container) return;

                // Все radio внутри контейнера
                const radios = container.querySelectorAll('input[type="radio"]');

                // Снимаем галочки
                radios.forEach(r => r.checked = false);

                // Для каждого имени создаём hidden input, если ещё нет
                const handledNames = new Set();
                radios.forEach(r => {
                    const name = r.name;
                    if (!handledNames.has(name)) {
                        handledNames.add(name);

                        // Проверяем, есть ли уже hidden
                        if (!container.querySelector(`input[type="hidden"][name="${name}"]`)) {
                            const hidden = document.createElement("input");
                            hidden.type = "hidden";
                            hidden.name = name;
                            hidden.value = ""; // передаст null на сервер
                            container.appendChild(hidden);
                        }
                    }
                });
            });
        });
    });
</script >