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

// Category Shop

const buttonEnable = document.querySelectorAll(".button-value");

buttonEnable.forEach((buttons) => {
  buttons.addEventListener("click", () => {
    document.querySelector(".btnActive")?.classList.remove("btnActive");
    buttons.classList.add("btnActive");
  });
});

// Category Blog

document.addEventListener("DOMContentLoaded", function () {
  const blogCategories = document.querySelectorAll(".categoryBlog");
  const selectedCategory = document.getElementById("selected");

  // Sayfa yüklendiğinde "All" kategorisi seçili olsun
  const initialCategory = document.querySelector(".selected");
  selectedCategory.textContent = initialCategory.textContent;

  blogCategories.forEach(category => {
      category.addEventListener("click", () => {
          // Diğer kategorilerden "selected" sınıfını kaldırın
          blogCategories.forEach(c => c.classList.remove("selected"));

          // Tıklanan kategoriye "selected" sınıfını ekleyin
          category.classList.add("selected");
          selectedCategory.textContent = category.textContent;
      });
  });
});


// Navbar

const navbarLinks = document.querySelectorAll("#navbar a");

document.addEventListener("DOMContentLoaded", () => {
  const currentPage = window.location.pathname.split("/").pop();
  navbarLinks.forEach((link) => {
    if (link.getAttribute("href") === currentPage) {
      link.classList.add("active");
    }
  });
});

// Contact us

const faqItems = document.querySelectorAll(".faq-item");

faqItems.forEach((faqs) => {
  faqs.addEventListener("click", () => {
    faqs.classList.toggle("faqActive");
  });
});

// document.getElementById("contact_us").addEventListener("click", function() {
//   // Kullanıcının adını, e-posta adresini ve mesajını al
//   var nameContact = prompt("Adınız:");
//   var emailContact = prompt("E-posta adresiniz:");
//   var messageContact = prompt("Mesajınız:");

//   // E-posta gönderimi için gereken verileri oluştur
//   var dataContact = {
//       nameContact: nameContact,
//       emailContact: emailContact,
//       messageContact: messageContact
//   };

//   // Sunucuya POST isteği gönder
//   fetch("ehedzade.huseyn@gmail.com", {
//       method: "POST",
//       headers: {
//           "Content-Type": "application/json"
//       },
//       body: JSON.stringify(dataContact)
//   })
//   .then(function(response) {
//       if (response.ok) {
//           alert("Mesajınız başarıyla gönderildi.");
//       } else {
//           alert("Mesaj gönderilirken bir hata oluştu.");
//       }
//   })
//   .catch(function(error) {
//       console.error("Hata:", error);
//   });
// });

//Add-To-Cart

// Sepet ürünleri için dizi
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

// Miktar değişikliklerini işle
function updateQuantity(event) {
  const newName = event.target.getAttribute("data-name");
  let newQuantity = parseInt(event.target.value);

  if (newQuantity <= 0) {
    showAlertModal("Quantity must be at least 1.");
    newQuantity = 1; // Minimum miktarı 1 olarak ayarla
  } else if (newQuantity > 100) {
    showAlertModal("Maximum quantity allowed is 100.");
    newQuantity = 100; // Maksimum miktarı 100 olarak ayarla
  }

  const itemToUpdate = cartItems.find((item) => item.name === newName);
  if (itemToUpdate) {
    itemToUpdate.quantity = newQuantity;
    updateCartHTML();
    saveCartItemsToLocalStorage();
    updateCartQuantity();
  }
}

// Sepet HTML'ini güncelleme işlevi
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
        <td><a href="#" class="remove-item" data-name="${
          item.name
        }"><i class="far fa-times-circle"></i></a></td>
        <td><img src="${item.image}" alt="" /></td>
        <td>${item.title}</td>
        <td>$${item.price.toFixed(2)}</td>
        <td><input type="number" value="${
          item.quantity
        }" min="1" max="100" data-name="${
        item.name
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

    // Miktar değişikliklerini dinle ve güncelle
    const quantityInputs = document.querySelectorAll(".quantity-input");
    quantityInputs.forEach((input) => {
      input.addEventListener("change", updateQuantity);
    });

    // Ürünleri silme işlevini dinle
    const removeItemLinks = document.querySelectorAll(".remove-item");
    removeItemLinks.forEach((link) => {
      link.addEventListener("click", removeItem);
    });
  }
}

// Sepet miktarını güncelleme işlevi
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

// Sayfa yüklendiğinde sepeti güncelle ve navbar'daki miktarı ayarla
window.addEventListener("load", () => {
  loadCartItemsFromLocalStorage();
  updateCartHTML();
  updateCartQuantity();
});

// Shop sayfasında "Add To Cart" butonlarına tıklama işlemi
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

  // 3 saniye sonra modalı kapat
  setTimeout(() => {
    document.body.removeChild(modal);
  }, 1800);
}



// "Add To Cart" butonuna tıklanınca çalışacak işlev
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
    // Hata durumunda input alanını işaretleyelim
    productSection.querySelector(".single-pro-details select").classList.add("selectSize-error");
    return;
  } else {
    // Hata yoksa kırmızı işareti kaldıralım
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

// Sepete ürün ekleme işlev
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

// Sepet ürünlerini yerel depolamaya kaydetme
function saveCartItemsToLocalStorage() {
  localStorage.setItem("cartItems", JSON.stringify(cartItems));
}

// Sepet ürünlerini yerel depolamadan yükleme
function loadCartItemsFromLocalStorage() {
  const storedCartItems = localStorage.getItem("cartItems");
  if (storedCartItems) {
    const parsedCartItems = JSON.parse(storedCartItems);
    cartItems.push(...parsedCartItems);
    updateCartHTML();
    updateCartQuantity();
  }
}

// Sepeti sıfırla ve mesaj göster
function checkout() {
  if (cartItems.length > 0) {
    cartItems.length = 0;
    updateCartHTML();
    saveCartItemsToLocalStorage();
    updateCartQuantity();
    showOrderConfirmationMessage();
  } else {
    // Sepet boşsa işlem yapma veya bir hata mesajı göster
    showAlertModal("Your cart is empty.");
  }
}

// Sipariş onay mesajını göster
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





