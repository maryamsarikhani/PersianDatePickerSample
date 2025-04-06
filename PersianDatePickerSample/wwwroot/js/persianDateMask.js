console.log("📅 Persian Date Picker JS Loaded");

window.addOutsideClickListener = function (pickerId, dotNetHelper) {
    function handleClickOutside(event) {
        const picker = document.getElementById(pickerId);
        if (picker && !picker.contains(event.target)) {
            dotNetHelper.invokeMethodAsync('ClosePicker');
        }
    }

    // Add the event listener to detect clicks anywhere in the document
    document.addEventListener('click', handleClickOutside);

    // Store the reference so you can remove it later (optional cleanup if needed)
    pickerId._outsideClickHandler = handleClickOutside;
};

window.clearInputAndApplyMask = function (inputId, dotNetHelper) {
    const input = document.getElementById(inputId);
    if (!input) return;

    // Check if input already has a valid date; don't reset
    const regex = /^\d{4}\/\d{2}\/\d{2}$/;
    if (!regex.test(input.value)) {
        input.value = "____/__/__";
    }

    input.addEventListener("input", () => {
        let digits = input.value.replace(/\D/g, "").slice(0, 8);
        let formatted = "";

        if (digits.length > 0) formatted += digits.slice(0, 4);
        if (digits.length > 4) formatted += "/" + digits.slice(4, 6);
        if (digits.length > 6) formatted += "/" + digits.slice(6, 8);

        input.value = formatted;

        if (digits.length === 8) {
            const year = parseInt(digits.slice(0, 4), 10);
            const month = parseInt(digits.slice(4, 6), 10);
            const day = parseInt(digits.slice(6, 8), 10);

            if (year >= 1380 && year <= 1410 && month >= 1 && month <= 12 && day >= 1 && day <= 31) {
                dotNetHelper.invokeMethodAsync("UpdateCalendar", year, month, day);
            } else {
                input.value = "____/__/__";
            }
        }
    });

    input.addEventListener("keydown", (e) => {
        const allowedKeys = ["Backspace", "Delete", "ArrowLeft", "ArrowRight", "Tab"];
        const isNumber = /^[0-9]$/.test(e.key);
        if (!isNumber && !allowedKeys.includes(e.key)) e.preventDefault();
    });

    input.addEventListener("blur", () => {
        const val = input.value;
        const regex = /^\d{4}\/\d{2}\/\d{2}$/;

        if (regex.test(val)) {
            const parts = val.split("/");
            const year = parseInt(parts[0], 10);
            const month = parseInt(parts[1], 10);
            const day = parseInt(parts[2], 10);

            if (year >= 1380 && year <= 1410 && month >= 1 && month <= 12 && day >= 1 && day <= 31) {
                dotNetHelper.invokeMethodAsync("UpdateCalendar", year, month, day);
            } else {
                input.value = "____/__/__";
            }
        }
    });
};

window.clearInput = function (inputId) {
    const input = document.getElementById(inputId);
    if (!input) return;

    input.value = "____/__/__";
};






