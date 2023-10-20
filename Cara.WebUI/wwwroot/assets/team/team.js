(function () {
  const grid = document.querySelector(".grid");
  let current = {
    index: -1,
    column: 0,
    line: 0,
  };
  const columns = 10;
  const easing = "cubic-bezier(0.165, 0.840, 0.440, 1.000)";
  const time = 400;

  grid.addEventListener("mouseenter", function (e) {
    const el = e.target;
    if (!el.classList.contains("grid-item")) return;

    const info = el.querySelector(".info");
    const image = el.querySelector("img");
    const index = Array.from(el.parentElement.children).indexOf(el);
    const column = index % columns;
    const line = Math.floor(index / columns);

    const item = {
      el: el,
      index: index,
      column: column,
      line: line,
      info: info,
      image: image,
    };

    if (current.el && current.index === index) return;

    if (line === current.line && column > current.column) {
      showItem(item, -100, 0, 25, 0);
      hideItem(current, 100, 0, -25, 0);
    } else if (line === current.line && column < current.column) {
      showItem(item, 100, 0, -25, 0);
      hideItem(current, -100, 0, 25, 0);
    } else if (line > current.line && column === current.column) {
      showItem(item, 0, -100, 0, 25);
      hideItem(current, 0, 100, 0, -25);
    } else {
      showItem(item, 0, 100, 0, -25);
      hideItem(current, 0, -100, 0, 25);
    }

    current = item;
  });

  function showItem(item, infoX, infoY, imageX, imageY) {
    item.info.style.transform = `translate(${infoX}%, ${infoY}%)`;
    item.info.style.display = "block";
    item.info.style.transition = `transform ${time}ms ${easing}`;

    item.image.style.transform = `translate(${imageX}%, ${imageY}%)`;
    item.image.style.opacity = 0.5;
    item.image.style.transition = `transform ${time}ms ${easing}, opacity ${time}ms ${easing}`;
  }

  function hideItem(item, infoX, infoY, imageX, imageY) {
    if (!item.el) return;

    item.info.style.transition = `transform ${time}ms ${easing}`;
    item.info.style.transform = `translate(${infoX}%, ${infoY}%)`;

    item.image.style.transition = `transform ${time}ms ${easing}, opacity ${time}ms ${easing}`;
    item.image.style.transform = `translate(${imageX}%, ${imageY}%)`;
    item.image.style.opacity = 1;
  }
})();
