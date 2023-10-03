var formSubmitted = false; 

document.addEventListener("DOMContentLoaded", function () {
	var submitBtn = document.getElementById("submitBtn");
	var productForm = document.getElementById("productForm");

	submitBtn.addEventListener("click", function () {
		if (!formSubmitted) { 
			formSubmitted = true; 
			submitBtn.disabled = true; 
			productForm.submit(); 
		}
	});
});