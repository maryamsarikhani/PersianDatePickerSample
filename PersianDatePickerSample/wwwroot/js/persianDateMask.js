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

            // Validate ranges without error messages
            if (year >= 1380 && year <= 1410 && month >= 1 && month <= 12 && day >= 1 && day <= 31) {
                dotNetHelper.invokeMethodAsync("UpdateCalendar", year, month, day); // Send valid data
            } else {
                input.value = "____/__/__"; // Reset invalid input
            }
        }
    });

    // Add event listener for keydown to restrict input to numbers and control keys
    input.addEventListener("keydown", (e) => {
        const allowedKeys = ["Backspace", "Delete", "ArrowLeft", "ArrowRight", "Tab"];
        const isNumber = /^[0-9]$/.test(e.key);
        if (!isNumber && !allowedKeys.includes(e.key)) e.preventDefault();
    });

    // Add event listener for blur to validate format and range
    input.addEventListener("blur", () => {
        const val = input.value;
        const regex = /^\d{4}\/\d{2}\/\d{2}$/;

        if (regex.test(val)) {
            const parts = val.split("/");
            const year = parseInt(parts[0], 10);
            const month = parseInt(parts[1], 10);
            const day = parseInt(parts[2], 10);

            // Validate ranges
            if (year >= 1380 && year <= 1410 && month >= 1 && month <= 12 && day >= 1 && day <= 31) {
                dotNetHelper.invokeMethodAsync("UpdateCalendar", year, month, day); // Send valid data
            } else {
                input.value = "____/__/__"; // Reset invalid input
            }
        }
    });
};
