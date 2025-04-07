console.log("📅 Persian Date Picker JS Loaded");

window.addEnterKeyListener = function (inputId, dotNetHelper) {
    const inputElement = document.getElementById(inputId);

    function handleKeyDown(event) {
        if (event.key === 'Enter') {
            // بررسی اینکه آیا ورودی خالی است یا معتبر نیست
            const regex = /^\d{4}\/\d{2}\/\d{2}$/;
            if (!regex.test(inputElement.value) || inputElement.value.trim() === "") {
                inputElement.value = "____/__/__";  // ورودی نامعتبر یا خالی است، خالی می‌شود
                dotNetHelper.invokeMethodAsync('ClosePicker');  // پیکر بسته می‌شود
            } else {
                dotNetHelper.invokeMethodAsync('ClosePicker');  // پیکر بسته می‌شود
            }
        }
    }

    inputElement.addEventListener('keydown', handleKeyDown);

    // Store the reference for cleanup (optional)
    inputId._enterKeyHandler = handleKeyDown;
};

window.registerClickOutside = function (pickerId, inputId, yearMonthSelectorId, dotNetHelper) {
    // Remove any existing listener to avoid duplicates
    document.removeEventListener('click', window._persianDatePickerClickOutsideHandler);

    window._persianDatePickerClickOutsideHandler = function (event) {
        const calendarPicker = document.getElementById(pickerId);
        const inputElement = document.getElementById(inputId);
        const yearMonthSelector = document.getElementById(yearMonthSelectorId);

        // Check if click is outside all relevant elements
        const clickedOutside =
            calendarPicker && !calendarPicker.contains(event.target) &&
            (!inputElement || !inputElement.contains(event.target)) &&
            (!yearMonthSelector || !yearMonthSelector.contains(event.target));

        if (clickedOutside) {
            dotNetHelper.invokeMethodAsync('ClosePicker');
        }
    };

    // Use capture phase to ensure we catch the event early
    document.addEventListener('click', window._persianDatePickerClickOutsideHandler, true);
};

window.removePersianDatePickerListeners = function (pickerId) {
    document.removeEventListener('click', window._persianDatePickerClickOutsideHandler);
    delete window._persianDatePickerClickOutsideHandler;

    const input = document.getElementById(`manualDateInput_${pickerId.split('_')[1]}`);
    if (input && input._enterKeyHandler) {
        input.removeEventListener('keydown', input._enterKeyHandler);
        delete input._enterKeyHandler;
    }
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
