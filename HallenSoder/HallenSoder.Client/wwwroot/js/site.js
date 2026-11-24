// Små hjälpfunktioner för Blazor-appen: scroll, reveal-animationer och meny-spy.
window.hallen = (() => {
  const io = new IntersectionObserver(
    (entries) => entries.forEach((e) => { if (e.isIntersecting) { e.target.classList.add("in"); io.unobserve(e.target); } }),
    { threshold: 0.12 }
  );
  let spy = null;

  window.addEventListener("scroll", () => document.body.classList.toggle("scrolled", window.scrollY > 40), { passive: true });

  return {
    // Läser vald bild från ett <input type=file>, förminskar den (max sida i px) och returnerar JPEG-bytes.
    async pickImage(input, maxSize) {
      const file = input.files && input.files[0];
      if (!file) return null;
      const bmp = await createImageBitmap(file);
      const scale = Math.min(1, maxSize / Math.max(bmp.width, bmp.height));
      const canvas = document.createElement("canvas");
      canvas.width = Math.round(bmp.width * scale);
      canvas.height = Math.round(bmp.height * scale);
      canvas.getContext("2d").drawImage(bmp, 0, 0, canvas.width, canvas.height);
      const blob = await new Promise((resolve) => canvas.toBlob(resolve, "image/jpeg", 0.85));
      input.value = "";
      return new Uint8Array(await blob.arrayBuffer());
    },
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
