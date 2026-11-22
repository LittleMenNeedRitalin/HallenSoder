// Små hjälpfunktioner för Blazor-appen: scroll, reveal-animationer och meny-spy.
window.hallen = (() => {
  const io = new IntersectionObserver(
    (entries) => entries.forEach((e) => { if (e.isIntersecting) { e.target.classList.add("in"); io.unobserve(e.target); } }),
    { threshold: 0.12 }
  );
  let spy = null;

  window.addEventListener("scroll", () => document.body.classList.toggle("scrolled", window.scrollY > 40), { passive: true });

  return {
    observeReveal() { document.querySelectorAll(".reveal:not(.in)").forEach((el) => io.observe(el)); },
    scrollToId(id) { document.getElementById(id)?.scrollIntoView({ behavior: "smooth" }); },
    initMenuSpy(ref) {
      spy?.disconnect();
      spy = new IntersectionObserver(
        (entries) => entries.forEach((e) => { if (e.isIntersecting) ref.invokeMethodAsync("SetActive", e.target.id); }),
        { rootMargin: "-30% 0px -60% 0px" }
      );
      document.querySelectorAll(".menu-section").forEach((s) => spy.observe(s));
    },
    disposeMenuSpy() { spy?.disconnect(); spy = null; },
    scrollTabIntoView() { document.querySelector(".tab.active")?.scrollIntoView({ inline: "center", block: "nearest" }); }
  };
})();
