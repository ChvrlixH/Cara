const bar = document.getElementById("bar");
const close = document.getElementById("close");
const nav = document.getElementById("navbar");

if (bar) {
    bar.addEventListener("click", () => {
        nav.classList.add("active");
    });
}

if (close) {
    close.addEventListener("click", () => {
        nav.classList.remove("active");
    });
}

var MainImg = document.getElementById("MainImg");
var smallImg = document.getElementsByClassName("small-img");

for (let i = 0; i < smallImg.length; i++) {
    smallImg[i].onclick = function () {
        MainImg.src = smallImg[i].src;
    };
}

// Search

const searchPro = () => {
    const searchbox = document.getElementById("search-item").value.toUpperCase();
    const storeitems = document.getElementById("pro-container");
    const product = document.querySelectorAll(".pro");
    const pname = storeitems.getElementsByTagName("h5");

    for (var i = 0; i < pname.length; i++) {
        let match = product[i].getElementsByTagName("h5")[0];

        if (match) {
            let textvalue = match.textContent || match.innerHTML;

            if (textvalue.toUpperCase().indexOf(searchbox) > -1) {
                product[i].style.display = "";
            } else {
                product[i].style.display = "none";
            }
        }
    }
};

//Category Shop

const buttonEnable = document.querySelectorAll(".button-value");

buttonEnable.forEach((buttons) => {
    buttons.addEventListener("click", () => {
        document.querySelector(".btnActive")?.classList.remove("btnActive");
        buttons.classList.add("btnActive");
    });
});




// Category Blog

document.addEventListener("DOMContentLoaded", function () {
    const selectedCategory = document.getElementById("selected");

    if (selectedCategory) {

        const initialCategory = document.querySelector(".selected");
        selectedCategory.textContent = initialCategory ? initialCategory.textContent : "";
    }

    const blogCategories = document.querySelectorAll(".categoryBlog");

    blogCategories.forEach(category => {
        category.addEventListener("click", () => {
            blogCategories.forEach(c => c.classList.remove("selected"));

            category.classList.add("selected");
            if (selectedCategory) {
                selectedCategory.textContent = category.textContent;
            }
        });
    });
});





// Product marquee

//document.querySelectorAll('.scrollable-text').forEach(function (textElement) {
//    if (textElement.scrollWidth > textElement.clientWidth) {
//        textElement.classList.add('marquee');
//    }
//});

//Blog Detail LoadMore
document.addEventListener("DOMContentLoaded", function () {
    const loadMoreDetailBtn = document.getElementById("loadMoreDetailBtn");
    const blogDetailList = document.getElementById("blogDetailList");
    const blogDetailCountElement = document.getElementById("blogDetailCount");

    if (blogDetailCountElement) {
        const blogDetailCount = blogDetailCountElement.value;
        let skip = 3;
        loadMoreDetailBtn.addEventListener("click", function () {
            console.log(skip);
            fetch(`/Blog/LoadMoreDetail?skip=${skip}`).then(response => response.text())
                .then(data => {
                    blogDetailList.innerHTML += data;
                })

            skip += 3;

            if (skip >= blogDetailCount) {
                loadMoreDetailBtn.remove();
            }
        })
    }
});










//Product Category







//Product Load More


function filterProducts(categoryId) {

    document.querySelectorAll('.pro').forEach(function (product) {
        product.style.display = 'none';
    });

    if (categoryId === null) {

        document.querySelectorAll('.pro').forEach(function (product) {
            product.style.display = 'block';
        });
    } else {

        document.querySelectorAll('.pro[data-category="' + categoryId + '"]').forEach(function (product) {
            product.style.display = 'block';
        });
    }
}


const productCountsByCategory = {};

document.addEventListener("DOMContentLoaded", function () {
    const loadMoreProductBtn = document.getElementById("loadMoreProductBtn");
    const productListLoadMore = document.getElementById("pro-container");
    const productCountElement = document.getElementById("productCountLoadMore");
    const buttons = document.querySelectorAll(".button-value");

    if (productCountElement) {
        const productCountLoadMore = productCountElement.value;
        let skip = 8;
        const increment = 8;

        buttons.forEach(function (button) {
            const categoryId = button.getAttribute("data-categoryShop-id");
            productCountsByCategory[categoryId] = productCountLoadMore;
        });

        loadMoreProductBtn.addEventListener("click", function () {
            const selectedCategoryId = getSelectedCategoryId();
            const productCountForCategory = productCountsByCategory[selectedCategoryId];
            if (productCountForCategory) {
                fetch(`/Shop/LoadMoreProduct?skip=${skip}&categoryId=${selectedCategoryId}`)
                    .then(response => response.text())
                    .then(data => {
                        productListLoadMore.innerHTML += data;
                    });

                skip += increment;

                if (skip >= productCountForCategory) {
                    loadMoreProductBtn.remove();
                }
            }
        });

        buttons.forEach(function (button) {
            button.addEventListener("click", function () {
                setCategoryActive(button);
                const categoryId = button.getAttribute("data-categoryShop-id");
                if (categoryId != "null") {
                    loadProductsByCategory(categoryId);
                } else {
                    loadAllProducts();
                }
            });
        });
    }

    function getSelectedCategoryId() {
        const activeCategoryButton = document.querySelector(".button-value.btnActive");
        return activeCategoryButton.getAttribute("data-categoryShop-id");
    }

    function setCategoryActive(button) {
        const activeCategoryButton = document.querySelector(".button-value.btnActive");
        if (activeCategoryButton) {
            activeCategoryButton.classList.remove("btnActive");
        }
        button.classList.add("btnActive");
    }

    function loadProductsByCategory(categoryId) {
        filterProducts(categoryId);
    }

    function loadAllProducts() {
        filterProducts(null);
    }
});


















// Navbar

/*const navbarLinks = document.querySelectorAll("#navbar a");*/

document.addEventListener("DOMContentLoaded", () => {
    const currentPage = window.location.pathname === "/"
        ? document.querySelector(`#navbar [data-page="Home"]`)
        : document.querySelector(`#navbar [data-page="${window.location.pathname.split("/").pop()}"]`);

    if (currentPage) {
        currentPage.classList.add("active");
    }
});



// Contact us

const faqItems = document.querySelectorAll(".faq-item");

faqItems.forEach((faqs) => {
    faqs.addEventListener("click", () => {
        faqs.classList.toggle("faqActive");
    });
});

// JavaScript dosyası

// Verileri cookieden alma 
function saveFormDataToCookie(data) {
    var jsonData = JSON.stringify(data);
    document.cookie = "formData=" + jsonData + "; expires=" + getCookieExpirationDate(24); // 24 saat
}

// Verileri cookieden oxuma
function loadFormDataFromCookie() {
    var cookieValue = getCookie("formData");
    if (cookieValue) {
        return JSON.parse(cookieValue);
    }
    return null;
}

// Cookienin vaxtını hesablama
function getCookieExpirationDate(hours) {
    var date = new Date();
    date.setTime(date.getTime() + (hours * 60 * 60 * 1000));
    return date.toUTCString();
}

// Cookieni alma
function getCookie(name) {
    var cookies = document.cookie.split(';');
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i].trim();
        if (cookie.startsWith(name + "=")) {
            return cookie.substring(name.length + 1);
        }
    }
    return "";
}

// Formu yükleme işlemi
// Form verilerini cookielere kaydetme işlevi
function saveFormDataToCookie(data) {
    // Verileri JSON formatında cookielere kaydet
    document.cookie = "formData=" + JSON.stringify(data);
}

// Cookielerden form verilerini yükleme işlevi
function loadFormDataFromCookie() {
    // "formData" adlı cookielerden verilerin alınması
    var cookieValue = document.cookie
        .split('; ')
        .find(row => row.startsWith("formData="));

    // JSON formatında verileri tehlil et
    if (cookieValue) {
        var data = cookieValue.split("=")[1];
        return JSON.parse(decodeURIComponent(data));
    }

    return null;
}

// Formu avtomatik olaraq yükleme işlevi
function loadForm() {
    var formData = loadFormDataFromCookie();

    if (formData) {
        document.getElementById("NameContact").value = formData.Name || "";
        document.getElementById("EmailContact").value = formData.Email || "";
        document.getElementById("SubjectContact").value = formData.Subject || "";
        document.getElementById("MessageContact").value = formData.Message || "";
    }
}

// Sayfa yüklendiğinde formu avtomatik olaraq yükle
window.addEventListener("load", function () {
    loadForm();
});

// Form gönderildiğinde bu işlevi çağır
var myForm = document.getElementById("myForm");
if (myForm) {
    myForm.addEventListener("submit", function (event) {
        event.preventDefault(); // Sayfanın yeniden yüklenmesini önle

        // Form verilerini al
        var name = document.getElementById("NameContact").value;
        var email = document.getElementById("EmailContact").value;
        var subject = document.getElementById("SubjectContact").value;
        var message = document.getElementById("MessageContact").value;

        // Verileri cookielere kaydet
        var data = {
            Name: name,
            Email: email,
            Subject: subject,
            Message: message
        };
        saveFormDataToCookie(data);

        if (!name || !email || !subject || !message) {
            alert("Tüm alanları doldurun!");
            return; // Veriler eksikse prosesi dayandır
        }
        /*window.location.href = "";*/

        document.getElementById("NameContact").value = "";
        document.getElementById("EmailContact").value = "";
        document.getElementById("SubjectContact").value = "";
        document.getElementById("MessageContact").value = "";

    });
}





//Add-To-Cart

// Sebet ürünleri üçün boş array
const cartItems = [];

// Ürünleri silme işlevi
function removeItem(event) {
    event.preventDefault();
    const link = event.target.closest(".remove-item");
    if (link) {
        const nameToRemove = link.getAttribute("data-name");
        const indexToRemove = cartItems.findIndex(
            (item) => item.name === nameToRemove
        );
        if (indexToRemove !== -1) {
            cartItems.splice(indexToRemove, 1);
            updateCartHTML();
            saveCartItemsToLocalStorage();
            updateCartQuantity();
        }
    }
}

// Miqdar değişikliklerini yoxla
function updateQuantity(event) {
    const newName = event.target.getAttribute("data-name");
    let newQuantity = parseInt(event.target.value);

    if (newQuantity <= 0) {
        showAlertModal("Quantity must be at least 1.");
        newQuantity = 1; // Minimum miqdarı 1 olaraq ayarla
    } else if (newQuantity > 100) {
        showAlertModal("Maximum quantity allowed is 100.");
        newQuantity = 100; // Maksimum miqdarı 100 olaraq ayarla
    }

    const itemToUpdate = cartItems.find((item) => item.name === newName);
    if (itemToUpdate) {
        itemToUpdate.quantity = newQuantity;
        updateCartHTML();
        saveCartItemsToLocalStorage();
        updateCartQuantity();
    }
}

// Sebet HTML'ini güncelleme işlevi
function updateCartHTML() {
    const cartTableBody = document.querySelector("#cart table tbody");
    const subtotalElement = document.querySelector("#subtotal table tbody");

    if (cartTableBody && subtotalElement) {
        cartTableBody.innerHTML = "";
        subtotalElement.innerHTML = "";

        let subtotal = 0;

        cartItems.forEach((item) => {
            const row = document.createElement("tr");
            const itemTotal = item.price * item.quantity;

            row.innerHTML = `
        <td><a href="#" class="remove-item" data-name="${item.name
                }"><i class="far fa-times-circle"></i></a></td>
        <td><img src="${item.image}" alt="" /></td>
        <td>${item.title}</td>
        <td>$${item.price.toFixed(2)}</td>
        <td><input type="number" value="${item.quantity
                }" min="1" max="100" data-name="${item.name
                }" class="quantity-input" /></td>
        <td class="item-total">$${itemTotal.toFixed(2)}</td>
      `;
            cartTableBody.appendChild(row);

            subtotal += itemTotal;
        });

        const subtotalRow = document.createElement("tr");
        subtotalRow.innerHTML = `
      <td>Cart Subtotal</td>
      <td class="subtotal-amount">$ ${subtotal.toFixed(2)}</td> 
    `;

        subtotalElement.appendChild(subtotalRow);

        const freeRow = document.createElement("tr");
        freeRow.innerHTML = `
    <td>Shipping</td>
    <td>Free</td>
    `;
        subtotalElement.appendChild(freeRow);

        const totalRow = document.createElement("tr");
        totalRow.innerHTML = `
      <td><strong>Total</strong></td>
      <td><strong class="total-amount">$ ${subtotal.toFixed(2)}</strong></td>
    `;
        subtotalElement.appendChild(totalRow);

        // Miqdar değişikliklerini oxu ve güncelle
        const quantityInputs = document.querySelectorAll(".quantity-input");
        quantityInputs.forEach((input) => {
            input.addEventListener("change", updateQuantity);
        });

        // Ürünleri silme işlevini oxu
        const removeItemLinks = document.querySelectorAll(".remove-item");
        removeItemLinks.forEach((link) => {
            link.addEventListener("click", removeItem);
        });
    }
}

// Sebet miqdarını güncelleme işlevi
function updateCartQuantity() {
    const totalQuantity = cartItems.reduce(
        (total, item) => total + item.quantity,
        0
    );

    const quantitySpans = document.querySelectorAll(".quantity");
    quantitySpans.forEach((span) => {
        span.textContent = totalQuantity;
    });
}

// Sayfa yüklendiğinde sebeti güncelle ve navbar'daki miqdarı ayarla
window.addEventListener("load", () => {
    loadCartItemsFromLocalStorage();
    updateCartHTML();
    updateCartQuantity();
});

// Shop sayfasında "Add To Cart" butonlarına klikleme işlemi
document.addEventListener("DOMContentLoaded", function () {
    const addToCartButtons = document.querySelectorAll("#prodetails .normal");

    addToCartButtons.forEach((button) => {
        button.addEventListener("click", addToCartClicked);
    });
});


function showAlertModal(message) {
    const modal = document.createElement("div");
    modal.className = "alert-modal";

    const modalContent = document.createElement("div");
    modalContent.className = "alert-modal-content";

    const alertMessage = document.createElement("p");
    alertMessage.className = "alert-message";
    alertMessage.textContent = message;

    modalContent.appendChild(alertMessage);
    modal.appendChild(modalContent);
    document.body.appendChild(modal);

    // 3 saniye sonra modalı bağla
    setTimeout(() => {
        document.body.removeChild(modal);
    }, 1800);
}



// "Add To Cart" butonuna kliklenende çalışacaq işlev
function addToCartClicked(event) {
    const button = event.target;
    const productSection = button.closest("#prodetails");
    const productImage = productSection.querySelector("#MainImg").src;
    const productName = productSection.querySelector(
        ".single-pro-details h4"
    ).textContent;
    const productPrice = parseFloat(
        productSection
            .querySelector(".single-pro-details h2")
            .textContent.replace("$", "")
    );
    const productQuantityInput = productSection.querySelector(".single-pro-details input[type='number']");
    const inputValue = parseInt(productQuantityInput.value);

    if (inputValue <= 0) {
        showAlertModal("Quantity must be at least 1.");
        productQuantityInput.value = "1";
        return;
    } else if (inputValue > 100) {
        showAlertModal("Maximum quantity allowed is 100.");
        productQuantityInput.value = "100";
        return;
    } else {
        productQuantityInput.value = inputValue.toString().replace(/^0+/, "");
    }

    const selectedSize = productSection.querySelector(
        ".single-pro-details select"
    ).value;

    if (selectedSize === "Select Size") {
        // Error veziyyetinde input alanını işaretle
        productSection.querySelector(".single-pro-details select").classList.add("selectSize-error");
        return;
    } else {
        // Error yoxdursa qırmızı işareti qaldır
        productSection.querySelector(".single-pro-details select").classList.remove("selectSize-error");
    }



    addToCart(
        productName,
        productPrice,
        parseInt(productQuantityInput.value),
        productImage,
        productName
    );
}

// Sebete ürün elave etme işlevi
function addToCart(productName, price, quantity, image, title) {
    const existingItem = cartItems.find((item) => item.name === productName);

    if (existingItem) {
        const availableSpace = 100 - existingItem.quantity;
        if (quantity <= availableSpace) {
            existingItem.quantity += quantity;
        } else {
            showAlertModal(`You can only add ${availableSpace} more of this item.`);
        }
    } else {
        cartItems.push({
            name: productName,
            price: price,
            quantity: quantity,
            image: image,
            title: title,
        });
    }

    updateCartHTML();
    saveCartItemsToLocalStorage();
    updateCartQuantity();
}

// Sebet ürünlerini daxili depolamaya kaydetme
function saveCartItemsToLocalStorage() {
    localStorage.setItem("cartItems", JSON.stringify(cartItems));
}

// Sebet ürünlerini daxili depolamadan yükleme
function loadCartItemsFromLocalStorage() {
    const storedCartItems = localStorage.getItem("cartItems");
    if (storedCartItems) {
        const parsedCartItems = JSON.parse(storedCartItems);
        cartItems.push(...parsedCartItems);
        updateCartHTML();
        updateCartQuantity();
    }
}

// Sebeti sıfırla ve mesaj göster
function checkout() {
    if (cartItems.length > 0) {
        cartItems.length = 0;
        updateCartHTML();
        saveCartItemsToLocalStorage();
        updateCartQuantity();
        showOrderConfirmationMessage();
    } else {
        // Sebet boşsa işlem etme veya bir error mesajı göster
        showAlertModal("Your cart is empty.");
    }
}

// Sifariş onay mesajını göster
function showOrderConfirmationMessage() {
    const modal = document.createElement("div");
    modal.className = "order-modal";

    const modalContent = document.createElement("div");
    modalContent.className = "order-modal-content";

    const message = document.createElement("p");
    message.className = "order-modal-message";
    message.textContent = "Your order has been successfully placed.";

    const closeButton = document.createElement("button");
    closeButton.className = "order-modal-button";
    closeButton.textContent = "Close";
    closeButton.addEventListener("click", () => {
        document.body.removeChild(modal);
    });

    modalContent.appendChild(message);
    modalContent.appendChild(closeButton);
    modal.appendChild(modalContent);
    document.body.appendChild(modal);
}







// Dark Mode 

// function addDarkmodeWidget() {
//   new Darkmode().showWidget();
// }
// window.addEventListener('load', addDarkmodeWidget);

const options = {
    bottom: '32px', // default: '32px'
    right: '32px', // default: '32px'
    left: 'unset', // default: 'unset'
    time: '0.6s', // default: '0.3s'
    mixColor: '#fff', // default: '#fff'
    backgroundColor: '#fff',  // default: '#fff'
    buttonColorDark: '#100f2c',  // default: '#100f2c'
    buttonColorLight: '#fff', // default: '#fff'
    saveInCookies: true, // default: true,
    label: '🌓', // default: ''
    autoMatchOsTheme: true // default: true
}

const darkmode = new Darkmode(options);
darkmode.showWidget();





