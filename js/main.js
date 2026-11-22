// Mobilmeny
const burger = document.querySelector(".burger");
const links = document.querySelector(".nav-links");
if (burger && links) {
  burger.addEventListener("click", () => {
    const open = links.classList.toggle("open");
    burger.setAttribute("aria-expanded", open);
  });
  links.addEventListener("click", (e) => {
    if (e.target.closest("a")) {
      links.classList.remove("open");
      burger.setAttribute("aria-expanded", "false");
    }
  });
}

// Header får bakgrund vid scroll
const header = document.querySelector(".site-header");
const onScroll = () => header.classList.toggle("scrolled", window.scrollY > 40);
onScroll();
window.addEventListener("scroll", onScroll, { passive: true });

// Reveal-animationer
const io = new IntersectionObserver(
  (entries) => entries.forEach((e) => {
    if (e.isIntersecting) { e.target.classList.add("in"); io.unobserve(e.target); }
  }),
  { threshold: 0.12 }
);
document.querySelectorAll(".reveal").forEach((el) => io.observe(el));

// Meny (data hämtad från hallen.one/meny)
const MENU = [
  {
    id: "lunch", title: "Lunch", note: "119 kr inkl. kaffe · fredagar 11–14",
    items: [
      ["Smashburger ost", "160g högrev – cheddarost, tomat, picklad rödlök, sallad, pommes & dipp"],
      ["Kycklingburgare", "Dubbelpanerad kycklingfilé – chili- & vitlöksmajo, tomat, rödlök, sallad, Grana Padano, pommes & dipp"],
      ["Pytt i panna", "Serveras med stekt ägg, rödbetor & saltgurka"],
      ["Raggmunk", "Serveras med stekt fläsk & råröda lingon"],
      ["Halloumiburgare", "Grillad halloumi – sirachamajo, tomat, sallad, rödlök, pommes & dipp"],
      ["Pasta pollo", "Krämig kyckling-pennepasta – champinjoner, lök, paprika, chili, vitlök, toppas med Grana Padano"],
    ],
  },
  {
    id: "smaratter", title: "Smårätter", items: [
      ["Nachos", "Paprika, rödlök, jalapeño, Grana Padano & chilimajo", 99],
      ["Toast Skagen", "Färska handskalade räkor, rödlök, citron, pepparrot & dill", 139],
      ["Kronärtskocka", "Friterad kronärtskocka med citronmajo & picklad rödlök", 139],
      ["Meaty fries", "Nötfärs, rödlök, paprika, jalapeño & chilimajo", 159],
      ["Meaty nachos deluxe", "Nötfärs, paprika, rödlök, jalapeño, Grana Padano & chilimajo", 169],
      ["Sportplanka", "4 st chili cheese, 4 st onion rings, 4 st mozzarella sticks & 4 st buffalo wings med dipp", 169],
      ["Hallens bricka", "2 olika ostar & 3 olika salami med oliver & grissini", 189],
      ["Pommes", "Serveras med dipp", 79],
      ["Sötpommes", "Serveras med flingsalt & dipp", 89],
      ["Vitlöksbröd", "Stekt vitlöksbröd med marinerade oliver", 89],
      ["Mozzarella sticks", "9 st, serveras med dipp", 89],
      ["Onion rings", "9 st, serveras med dipp", 89],
      ["Chili cheese", "9 st, serveras med dipp", 89],
      ["Buffalo wings", "9 st med paprika, rödlök, jalapeños, BBQ-sås & dipp", 99],
    ],
  },
  {
    id: "varmratter", title: "Varmrätter", items: [
      ["Fläskkarré", "Marinerad fläskkarré – rödvinssås, bearnaise & pommes", 179],
      ["Club sandwich", "Grillad kyckling med bacon; sirachamajo, tomat, picklad rödlök, sallad, pommes & dipp", 189],
      ["Smashburger ost", "160g högrev; dubbel cheddar, tomat, picklad rödlök, sallad, pommes & dipp (bacon +10 kr)", 179],
      ["Svamppasta", "Krämig pasta med champinjoner, kantareller, portobello, silverlök, vitlök, paprika & vitvin", 179],
      ["Biffpasta", "Krämig biffpasta; champinjoner, lök, paprika, vitlök & konjak", 199],
      ["Souvlakispett", "Kyckling – serveras med pommes, tzatziki & grekisk sallad", 179],
      ["Rödspätta", "Panerad rödspätta med pommes, remouladsås & citron", 179],
      ["Kycklingburgare", "Dubbelpanerad kycklingfilé; chili, vitlöksmajo, tomat, rödlök, sallad, Grana Padano, pommes & dipp", 189],
      ["Pytt i panna", "Serveras med stekt ägg, rödbetor & saltgurka", 179],
      ["Halloumiburgare", "Grillad halloumi; sirachamajo, tomat, sallad, rödlök, pommes & dipp", 179],
      ["Pasta pollo", "Krämig kyckling-pennepasta; champinjoner, lök, paprika, chili, vitlök & Grana Padano", 179],
      ["Black Angus ryggbiff", "Rosmarin- & vitlöksmarinerad. Serveras med stekpotatis/pommes/sidsallad, rödvinssås & bearnaise", 219],
      ["Söder sallad", "Grillad kyckling & halloumi. Grönsaker med Grana Padano, krutonger & Hallens dressing", 199],
      ["Schnitzel", "Panerat fläsk. Serveras med stekpotatis, rödvinssås, bearnaise, citron & kapris", 189],
    ],
  },
  {
    id: "efterratter", title: "Efterrätter", items: [
      ["Chokladfondant", "Serveras med vaniljglass", 99],
      ["Vaniljglass", "3 kulor, serveras med choklad- eller colasås", 79],
    ],
  },
  {
    id: "dessertdrinkar", title: "Dessertdrinkar", items: [
      ["Espresso martini", "Vodka, Kahlúa, kaffe", 149],
      ["Kaffe Karlsson", "Baileys, apelsinlikör, kaffe, grädde", 149],
      ["Irish coffee", "Whisky, kaffe, farinsocker, grädde", 149],
      ["Rosa pantern", "Vodka, Likör 43, mjölk, grenadin", 149],
    ],
  },
];

const root = document.getElementById("menu-root");
if (root) {
  const tabs = document.getElementById("menu-tabs");
  tabs.innerHTML = MENU.map(
    (s, i) => `<button class="tab${i === 0 ? " active" : ""}" data-target="${s.id}">${s.title}</button>`
  ).join("");

  root.innerHTML = MENU.map((s) => `
    <section class="menu-section" id="${s.id}">
      <h2>${s.title}</h2>
      ${s.note ? `<p class="note">${s.note}</p>` : ""}
      <div class="menu-grid">
        ${s.items.map(([name, desc, price]) => `
          <article class="dish">
            <div class="dish-top"><h3>${name}</h3>${price ? `<span class="price">${price} kr</span>` : ""}</div>
            <p>${desc}</p>
          </article>`).join("")}
      </div>
    </section>`).join("");

  tabs.addEventListener("click", (e) => {
    const b = e.target.closest(".tab");
    if (b) document.getElementById(b.dataset.target).scrollIntoView({ behavior: "smooth" });
  });

  const spy = new IntersectionObserver(
    (entries) => entries.forEach((e) => {
      if (e.isIntersecting) {
        tabs.querySelectorAll(".tab").forEach((t) => t.classList.toggle("active", t.dataset.target === e.target.id));
        tabs.querySelector(".active")?.scrollIntoView({ inline: "center", block: "nearest" });
      }
    }),
    { rootMargin: "-30% 0px -60% 0px" }
  );
  root.querySelectorAll(".menu-section").forEach((s) => spy.observe(s));
}
