#!/usr/bin/env bash
# Genererar HTML-sidorna (delad header/footer). Kör: bash _build/build.sh
set -e
cd "$(dirname "$0")/.."

cur() { [ "$1" = "$2" ] && printf ' aria-current="page"' || true; }

HEAD() { # $1=title $2=desc $3=active page $4=header class
cat <<EOF
<!doctype html>
<html lang="sv">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title>$1</title>
<meta name="description" content="$2">
<meta property="og:title" content="$1">
<meta property="og:description" content="$2">
<meta property="og:image" content="img/interior-bar.webp">
<meta name="theme-color" content="#07191c">
<link rel="icon" href="img/logo.png">
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Barlow+Condensed:wght@600;700;800&family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet">
<link rel="stylesheet" href="css/style.css">
</head>
<body>
<header class="site-header $4">
  <div class="wrap nav">
    <a class="logo" href="index.html" aria-label="Hallen Södermalm – startsida"><img src="img/logo.png" alt="Hallen"></a>
    <button class="burger" aria-label="Meny" aria-expanded="false"><span></span><span></span><span></span></button>
    <ul class="nav-links">
      <li><a href="index.html"$(cur "$3" home)>Start</a></li>
      <li><a href="meny.html"$(cur "$3" meny)>Meny</a></li>
      <li><a href="kontakt.html"$(cur "$3" kontakt)>Hitta hit</a></li>
      <li><a class="btn btn-primary" href="boka-bord.html">Boka bord</a></li>
    </ul>
  </div>
</header>
EOF
}

FOOT() {
cat <<'EOF'
<footer class="site-footer">
  <div class="wrap">
    <div class="foot-grid">
      <div>
        <img src="img/logo.png" alt="Hallen Södermalm">
        <p>Din moderna sportsbar på Södermalm – live-sport, god mat och fräscha drycker.</p>
      </div>
      <div>
        <h4>Sidor</h4>
        <a href="index.html">Start</a>
        <a href="meny.html">Meny</a>
        <a href="boka-bord.html">Boka bord</a>
        <a href="kontakt.html">Hitta hit</a>
      </div>
      <div>
        <h4>Kontakt</h4>
        <p>Götgatan 85A<br>116 62 Stockholm</p>
        <a href="tel:08828265">08-828 265</a>
        <a href="mailto:info@hallen.one">info@hallen.one</a>
      </div>
    </div>
    <div class="copy"><span>© <span id="yr">2026</span> Hallen Södermalm</span><span>Prototyp – redesign av hallen.one</span></div>
  </div>
</footer>
<script src="js/main.js"></script>
<script>document.getElementById("yr").textContent = new Date().getFullYear();</script>
</body>
</html>
EOF
}

# ---------- index ----------
{ HEAD "Hallen Södermalm – Sportsbar på Götgatan" "Hallen Södermalm: modern sportsbar med live-sport på storbildsskärm, god mat och fräscha drycker. Boka bord idag." home ""
cat <<'EOF'
<main>
<section class="hero">
  <div class="wrap">
    <span class="eyebrow">Sportsbar · Södermalm · Stockholm</span>
    <h1>För sport<span>och skoj!</span></h1>
    <p class="lead">Välkommen till Hallen Södermalm – den ultimata platsen för dig som älskar sport, god mat och trevlig atmosfär.</p>
    <div class="hero-cta">
      <a class="btn btn-primary" href="boka-bord.html">Boka bord</a>
      <a class="btn btn-ghost" href="meny.html">Se menyn</a>
    </div>
  </div>
</section>

<div class="strip">
  <div class="wrap">
    <div class="strip-item"><span class="ico">📍</span><div><small>Hitta hit</small><strong>Götgatan 85A, Södermalm</strong></div></div>
    <div class="strip-item"><span class="ico">📺</span><div><small>På skärmarna</small><strong>All sport, live</strong></div></div>
    <div class="strip-item"><span class="ico">🍔</span><div><small>Lunch</small><strong>119 kr inkl. kaffe</strong></div></div>
  </div>
</div>

<section>
  <div class="wrap split">
    <div class="reveal">
      <span class="eyebrow">Hallen Södermalm</span>
      <h2>Din moderna sportsbar som sänder all sport</h2>
      <p class="lead">Upplev spänningen med live-matcher på storbildsskärm. Njut av din favoritsport tillsammans med vänner i en pulserande atmosfär – mitt i hjärtat av Stockholm.</p>
      <a class="btn btn-ghost" href="kontakt.html">Hitta hit</a>
    </div>
    <img class="reveal" src="img/bar-counter.webp" alt="Baren på Hallen med skärmar och hängande glas" loading="lazy">
  </div>
</section>

<section style="background:var(--bg-2)">
  <div class="wrap">
    <div class="section-head reveal"><span class="eyebrow">Upptäck</span><h2>Mat, dryck &amp; matcher</h2></div>
    <div class="cards">
      <a class="card reveal" href="meny.html">
        <img src="img/food.webp" alt="Meaty fries och burgare" loading="lazy">
        <div class="card-body"><h3>God &amp; variation</h3><p>Från saftiga hamburgare till läckra vegetariska alternativ – något för varje smak.</p><span class="card-link">Se vår meny</span></div>
      </a>
      <a class="card reveal" href="meny.html#dessertdrinkar">
        <img src="img/drink.webp" alt="Aperol Spritz på bardisken" loading="lazy">
        <div class="card-body"><h3>Fräscha drycker</h3><p>Klassiska ölsorter, local craft beer och läckra alkoholfria alternativ.</p><span class="card-link">Dryckesmeny</span></div>
      </a>
      <a class="card reveal" href="boka-bord.html">
        <img src="img/interior-lounge.webp" alt="Lokalen med soffor och storbildsskärmar" loading="lazy">
        <div class="card-body"><h3>Live-matcher</h3><p>Storbildsskärmar och full stämning. Boka bord och ta med kompisarna.</p><span class="card-link">Boka bord</span></div>
      </a>
    </div>
  </div>
</section>

<section>
  <div class="wrap">
    <div class="section-head reveal"><span class="eyebrow">Stämningen</span><h2>Kika in hos oss</h2></div>
    <div class="gallery reveal">
      <img class="wide" src="img/interior-bar.webp" alt="Hallens bar och lokal" loading="lazy">
      <img class="tall" src="img/interior-dark.webp" alt="Barstolar och soffor mot entrén" loading="lazy">
      <img src="img/drink.webp" alt="Aperol Spritz" loading="lazy">
      <img src="img/food.webp" alt="Meaty fries" loading="lazy">
      <img class="wide" src="img/interior-lounge.webp" alt="Soffgrupper och bar" loading="lazy">
    </div>
  </div>
</section>

<section class="band">
  <div class="wrap reveal">
    <span class="eyebrow">Afterwork</span>
    <h2>Boka er afterwork hos oss</h2>
    <p class="lead">Samla jobbet, laget eller kompisgänget. Mat, dryck och sport i en och samma lokal.</p>
    <a class="btn btn-primary" href="boka-bord.html">Boka bord</a>
  </div>
</section>
</main>
EOF
FOOT; } > index.html

# ---------- meny ----------
{ HEAD "Meny – Hallen Södermalm" "Hela menyn på Hallen Södermalm: lunch, smårätter, varmrätter, efterrätter och dessertdrinkar." meny solid
cat <<'EOF'
<main>
<section class="page-hero">
  <div class="wrap">
    <span class="eyebrow">Upptäck</span>
    <h2>Vår meny</h2>
    <p class="lead" style="margin-top:1rem">Husmanskost, burgare och barmat med hög nivå. Lunch serveras för 119 kr inklusive kaffe.</p>
  </div>
</section>
<div class="menu-tabs"><div class="wrap"><div class="tabs-inner" id="menu-tabs"></div></div></div>
<section style="padding-top:0">
  <div class="wrap">
    <div id="menu-root"><noscript><p class="lead">Aktivera JavaScript för att se menyn, eller besök <a href="https://hallen.one/meny/">hallen.one/meny</a>.</p></noscript></div>
    <div class="menu-banner"><img src="img/food.webp" alt="Meaty fries och burgare" loading="lazy"></div>
    <p class="menu-foot">Har du allergier eller specialkost? Säg till din servitör. Priser i SEK. Vill du ha dryckesmenyn? Fråga i baren eller ring 08-828 265.</p>
    <p style="margin-top:2rem"><a class="btn btn-primary" href="boka-bord.html">Boka bord</a></p>
  </div>
</section>
</main>
EOF
FOOT; } > meny.html

# ---------- boka-bord ----------
{ HEAD "Boka bord – Hallen Södermalm" "Boka bord på Hallen Södermalm, Götgatan 85A. Bokning gäller för ätande gäster." boka solid
cat <<'EOF'
<main>
<section class="page-hero">
  <div class="wrap">
    <span class="eyebrow">Möt upp</span>
    <h2>Boka bord</h2>
    <p class="lead" style="margin-top:1rem">Säkra ditt bord inför matchen, lunchen eller afterworken.</p>
  </div>
</section>
<section style="padding-top:3rem">
  <div class="wrap booking-grid">
    <div class="reveal">
      <div class="alert"><strong>Observera:</strong> bokning av bord gäller enbart för ätande gäster.</div>
      <div class="iframe-box"><iframe src="https://widget.thefork.com/sv/550750af-69ba-4857-9dee-b3fa04c2f0ba" title="Boka bord via TheFork" loading="lazy"></iframe></div>
    </div>
    <aside class="panel reveal">
      <h3>Hellre ringa?</h3>
      <ul class="list" style="margin-top:1.2rem">
        <li><small>Telefon</small><a href="tel:08828265">08-828 265</a></li>
        <li><small>E-post</small><a href="mailto:info@hallen.one">info@hallen.one</a></li>
        <li><small>Adress</small>Götgatan 85A, 116 62 Stockholm</li>
      </ul>
      <p style="margin-top:1.5rem;color:var(--muted)">Större sällskap eller afterwork? Hör av dig så fixar vi det.</p>
    </aside>
  </div>
</section>
</main>
EOF
FOOT; } > boka-bord.html

# ---------- kontakt ----------
{ HEAD "Hitta hit – Hallen Södermalm" "Hitta till Hallen Södermalm på Götgatan 85A i Stockholm. Telefon 08-828 265." kontakt solid
cat <<'EOF'
<main>
<section class="page-hero">
  <div class="wrap">
    <span class="eyebrow">Kontakt</span>
    <h2>Hitta hit</h2>
    <p class="lead" style="margin-top:1rem">Vi finns på Götgatan på Södermalm.</p>
  </div>
</section>
<section style="padding-top:3rem">
  <div class="wrap contact-grid">
    <div class="panel reveal">
      <h3>Hallen Södermalm</h3>
      <ul class="list" style="margin-top:1.4rem">
        <li><small>Adress</small>Götgatan 85A<br>116 62 Stockholm</li>
        <li><small>Telefon</small><a href="tel:08828265">08-828 265</a></li>
        <li><small>E-post</small><a href="mailto:info@hallen.one">info@hallen.one</a></li>
      </ul>
      <p style="margin-top:2rem"><a class="btn btn-primary" href="boka-bord.html">Boka bord</a></p>
    </div>
    <div class="map reveal">
      <iframe title="Karta till Hallen Södermalm" loading="lazy" src="https://maps.google.com/maps?q=G%C3%B6tgatan%2085A%20116%2062%20Stockholm&amp;t=m&amp;z=15&amp;output=embed&amp;iwloc=near"></iframe>
    </div>
  </div>
</section>
</main>
EOF
FOOT; } > kontakt.html

echo "Klart:"; ls *.html
