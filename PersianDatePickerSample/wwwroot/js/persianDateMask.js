console.log("📅 Persian Date Picker JS Loaded");

window.addOutsideClickListener = (pickerElementId, dotNetHelper) => {
    document.addEventListener('click', function handleClickOutside(event) {
        const picker = document.getElementById(pickerElementId);
        if (picker && !picker.contains(event.target)) {
            dotNetHelper.invokeMethodAsync('ClosePicker');
        }
    });
};

window.clearInputAndApplyMask = function (inputId, dotNetHelper) {
    const input = document.getElementById(inputId);
    if (!input) return;

    // Reset the input value to the placeholder mask
    input.value = "____/__/__";

    // Add an event listener to handle input completion from left to right
    input.addEventListener("input", () => {
        let digits = input.value.replace(/\D/g, "").slice(0, 8);
        let formatted = "";

        if (digits.length > 0) formatted += digits.slice(0, 4); // Year
        if (digits.length > 4) formatted += "/" + digits.slice(4, 6); // Month
        if (digits.length > 6) formatted += "/" + digits.slice(6, 8); // Day

        input.value = formatted;

        // Validate input on completion (if input is fully entered)
        if (digits.length === 8) {
            const year = parseInt(digits.slice(0, 4), 10);
            const month = parseInt(digits.slice(4, 6), 10);
            const day = parseInt(digits.slice(6, 8), 10);

            // ارسال اطلاعات به Blazor برای بررسی‌های دقیق‌تر و به‌روزرسانی تقویم
            input.setCustomValidity(""); // Clear errors
            dotNetHelper.invokeMethodAsync("UpdateCalendar", year, month, day);
        }
    });

    // Add event listener for keydown to restrict input to numbers and control keys
    input.addEventListener("keydown", (e) => {
        const allowedKeys = ["Backspace", "Delete", "ArrowLeft", "ArrowRight", "Tab"];
        const isNumber = /^[0-9]$/.test(e.key);
        if (!isNumber && !allowedKeys.includes(e.key)) e.preventDefault();
    });

    // Add event listener for blur to validate format
    input.addEventListener("blur", () => {
        const val = input.value;
        const regex = /^\d{4}\/\d{2}\/\d{2}$/;
        if (!regex.test(val)) {
            input.setCustomValidity("فرمت تاریخ باید به صورت yyyy/MM/dd باشد.");
        } else {
            input.setCustomValidity(""); // Clear errors if format is correct
        }
    });
};




//window.applyPersianDateMask = function (inputId) {
//    const input = document.getElementById(inputId);
//    if (!input) return;

//    input.addEventListener("input", () => {
//        let digits = input.value.replace(/\D/g, "").slice(0, 8);
//        let formatted = "";

//        if (digits.length > 0) formatted += digits.slice(0, 4);
//        if (digits.length > 4) formatted += "/" + digits.slice(4, 6);
//        if (digits.length > 6) formatted += "/" + digits.slice(6, 8);

//        input.value = formatted;
//    });

//    input.addEventListener("keydown", (e) => {
//        const allowedKeys = ["Backspace", "Delete", "ArrowLeft", "ArrowRight", "Tab"];
//        const isNumber = /^[0-9]$/.test(e.key);
//        if (!isNumber && !allowedKeys.includes(e.key)) e.preventDefault();
//    });

//    input.addEventListener("blur", () => {
//        const val = input.value;
//        const regex = /^\d{4}\/\d{2}\/\d{2}$/;
//        if (!regex.test(val)) {
//            input.setCustomValidity("فرمت تاریخ باید به صورت yyyy/MM/dd باشد.");
//        } else {
//            input.setCustomValidity("");
//        }
//    });
//};
