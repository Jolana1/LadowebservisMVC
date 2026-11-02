/* Ladowebservis Live Chat - nákupný poradca */
(function () {
    'use strict';

    var autoResponses = {
        cena: '💰 <strong>Ceny a ponuka</strong><br>Aktuálne ceny nájdeš v sekcii <strong>Produkty</strong>.',
        produkty: '🛍️ <strong>Pomôžem ti vybrať produkt.</strong><br>Napíš napríklad „Omega-3“, „kolagén“, „vitamín D“ alebo „BalanceTest“.',
        poradca: '🤝 <strong>Nákupný poradca</strong><br>Napíš, čo hľadáš alebo aký problém chceš riešiť a pomôžem ti zorientovať sa v ponuke.',
        zlava: '🍂 <strong>Jesenná akcia – JESENJETU26: zľava 10 %</strong><br>Na označené produkty v košíku platí zľava <strong>10 %</strong>. Zľava sa v košíku už automaticky odpočítava. Pri platbe cez <strong>Stripe</strong> použi zľavový kód <strong>JESENJETU26</strong> podľa pokynov v košíku.<br><br><strong>Produkty so zľavou:</strong> BalanceTest (ID 0), BalanceOil+ Orange 300 ml (ID 1), ZinoBiotic+ (ID 2), ZinzinoXtend (ID 3), Vitamin D Test (ID 5), CollagenBoozt (ID 8) a BalancePack 4-in-1 (ID 9).<br><br>Ostatné produkty, vrátane ID 10–13, do tejto akcie nepatria. Kód nie je ďalšia zľava navyše k už zobrazenej 10 % zľave v košíku.',
        doprava: '🚚 <strong>Doprava</strong><br>Cena dopravy sa vypočíta podľa zvolenej služby a hodnoty košíka.',
        platba: '💳 <strong>Platba</strong><br>Platbu môžeš vykonať bezpečne cez Stripe.',
        reklamacia: '🛠️ <strong>Chcem riešiť reklamáciu</strong><br>Mrzí nás, že s tovarom niečo nie je v poriadku. Reklamáciu nám nahlás na <strong>podpora@ladowebservis.sk</strong>. Uveď číslo objednávky, názov produktu, opis vady a podľa možnosti prilož fotografie.<br><br>Podrobné podmienky a postup nájdeš v <a href="/return-policy" target="_blank" rel="noopener noreferrer">Zásadách vrátenia tovaru a reklamácií</a>.<br><br>Ak chceš tovar len vrátiť bez udania dôvodu, napíš „Chcem vrátiť tovar“ – ide o iný postup.',
        vratenie: '↩️ <strong>Chcem vrátiť tovar</strong><br>Ak si spotrebiteľ a nakúpil si na diaľku, spravidla môžeš odstúpiť od zmluvy do <strong>14 dní od doručenia</strong> bez uvedenia dôvodu; platia zákonné výnimky.<br><br>Napíš na <strong>podpora@ladowebservis.sk</strong>, uveď číslo objednávky a produkt, ktorý chceš vrátiť. Presný postup a podmienky si over v <a href="/return-policy" target="_blank" rel="noopener noreferrer">Zásadách vrátenia tovaru</a>.<br><br>Ak je produkt chybný alebo poškodený, napíš „Chcem riešiť reklamáciu“.',
        kontakt: '📞 <strong>Kontakt</strong><br>☎️ +421907151293<br>📧 info@ladowebservis.sk',
        it: '💻 <strong>IT služby</strong><br>Pomôžeme s počítačmi, notebookmi, Windows, SSD, RAM, servisom, nastavením a riešením problémov.',
        pomoc: '❓ <strong>Ako ti môžem pomôcť?</strong><br>Skús: produkty, poradca, akcie a zľavy, JESENJETU26, doprava, platba, reklamácia alebo kontakt.'
    };

    function esc(s) {
        var d = document.createElement('div');
        d.textContent = s;
        return d.innerHTML;
    }

    function response(q) {
        q = q.toLowerCase().trim();
        var normalized = q.normalize('NFD').replace(/[\u0300-\u036f]/g, '');
        if (autoResponses[q]) return autoResponses[q];
        if (/(jesenjetu26|jesenn|akci|zlav|kupon|promo.?kod|zlavy)/.test(normalized))
            return autoResponses.zlava;

        if (q.indexOf('balanceoil') >= 0 || q.indexOf('omega') >= 0 || q.indexOf('olej') >= 0)
            return '⭐ <strong>BalanceOil+ Orange</strong><br>Ak hľadáš Omega-3 olej, pozri si BalanceOil+ Orange 300 ml alebo 100 ml.';
        if (q.indexOf('zinobiotic') >= 0 || q.indexOf('mikrobiom') >= 0)
            return '🦠 <strong>ZinoBiotic+</strong><br>Produkt zameraný na podporu črevného mikrobiómu.';
        if (q.indexOf('kolagen') >= 0 || q.indexOf('collagen') >= 0)
            return '✨ <strong>CollagenBoozt</strong><br>Tekutý morský kolagén v praktickom balení.';
        if (q.indexOf('vitamin d') >= 0 || q.indexOf('vitamín d') >= 0)
            return '🧪 <strong>Vitamin D Test</strong><br>Ak hľadáš test vitamínu D, pozri si Vitamin D Test v ponuke.';
        if (/(reklam|poskoden|poskodeny produkt|vadn|vadny tovar|pokazen|nefunguj|chybn|vada|zaruk)/.test(normalized))
            return autoResponses.reklamacia;
        if (/(vrat(it|enie)?|odstup|14 dni|peniaze spat|refund)/.test(normalized))
            return autoResponses.vratenie;
        if (q.indexOf('kurier') >= 0 || q.indexOf('kuriér') >= 0 || q.indexOf('packeta') >= 0 || q.indexOf('zásiel') >= 0)
            return autoResponses.doprava;
        if (q.indexOf('it ') >= 0 || q.indexOf('it služ') >= 0 || q.indexOf('it sluz') >= 0 || q.indexOf('počítač') >= 0 || q.indexOf('pocitac') >= 0 || q.indexOf('notebook') >= 0 || q.indexOf('windows') >= 0 || q.indexOf('ssd') >= 0 || q.indexOf('ram') >= 0 || q.indexOf('servis') >= 0)
            return autoResponses.it;
        if (q.indexOf('cena') >= 0 || q.indexOf('koľko') >= 0 || q.indexOf('kolko') >= 0)
            return autoResponses.cena;

        return '😊 <strong>Rád poradím.</strong><br>Skús „potrebujem Omega-3“, „aká je zľava?“, „ako reklamovať?“ alebo „aké produkty máte?“';
    }

    function addStyles() {
        if (document.getElementById('lwsLiveChatStyles')) return;
        var s = document.createElement('style');
        s.id = 'lwsLiveChatStyles';
        s.textContent = `
#lwsLiveChat{font-family:Arial,Helvetica,sans-serif;z-index:99999}
.lws-launch{position:fixed;right:22px;bottom:22px;width:62px;height:62px;border:0;border-radius:50%;background:#111827;color:#fff;cursor:pointer;box-shadow:0 8px 28px rgba(0,0,0,.25);z-index:99999}
.lws-launch:hover{transform:translateY(-2px)}
.lws-launch .dot{position:absolute;right:2px;top:2px;width:13px;height:13px;border-radius:50%;background:#22c55e;border:2px solid #fff}
.lws-window{position:fixed;right:22px;bottom:96px;width:370px;max-width:calc(100vw - 30px);height:570px;max-height:calc(100vh - 115px);background:#fff;border:1px solid #e5e7eb;border-radius:18px;box-shadow:0 18px 55px rgba(0,0,0,.25);overflow:hidden;display:none;flex-direction:column;z-index:100000}
.lws-window.open{display:flex}
.lws-head{background:#111827;color:#fff;padding:13px;display:flex;justify-content:space-between;align-items:center}
.lws-brand{display:flex;align-items:center;gap:10px}.lws-brand img{width:42px;height:42px;object-fit:contain;background:#fff;border-radius:9px;padding:4px}.lws-brand strong{display:block}.lws-brand small{opacity:.8}
.lws-close{border:0;background:transparent;color:#fff;font-size:28px;cursor:pointer}
.lws-intro{padding:13px;background:#f8fafc;font-size:13px}.lws-intro p{margin:5px 0 0}
.lws-actions{padding:9px;display:grid;grid-template-columns:1fr 1fr;gap:6px;background:#f8fafc;border-bottom:1px solid #e5e7eb}
.lws-actions button{padding:8px 5px;border:1px solid #dbe2ea;border-radius:9px;background:#fff;cursor:pointer;font-size:12px}
.lws-msgs{flex:1;overflow-y:auto;padding:12px}.lws-msg{max-width:88%;padding:9px 11px;border-radius:12px;margin-bottom:9px;font-size:13px;line-height:1.45}.lws-bot{background:#f1f5f9}.lws-user{margin-left:auto;background:#111827;color:#fff}
.lws-input{display:flex;gap:7px;padding:10px;border-top:1px solid #e5e7eb}.lws-input input{flex:1;min-width:0;border:1px solid #d1d5db;border-radius:10px;padding:10px}.lws-input button{width:42px;border:0;border-radius:10px;background:#111827;color:#fff;cursor:pointer}
.lws-foot{text-align:center;font-size:10px;color:#94a3b8;padding:5px 8px 8px}
#lwsBackTop{position:fixed;right:22px;bottom:96px;width:42px;height:42px;border:0;border-radius:50%;background:#111827;color:#fff;cursor:pointer;box-shadow:0 6px 20px rgba(0,0,0,.22);z-index:99998;display:none;font-size:20px}
#lwsBackTop.show{display:block}
@media(max-width:520px){#lwsBackTop{right:15px;bottom:88px;width:40px;height:40px}}
@media(max-width:520px){.lws-launch{right:15px;bottom:15px}.lws-window{right:10px;bottom:87px;width:calc(100vw - 20px);height:min(570px,calc(100vh - 105px))}}
`;
        document.head.appendChild(s);
    }

    function create() {
        if (document.getElementById('lwsLiveChat')) return;
        var w = document.createElement('div');
        w.id = 'lwsLiveChat';
        w.innerHTML = `
<button class="lws-launch" id="lwsOpen" title="Nákupný poradca">💬<span class="dot"></span></button>
<section class="lws-window" id="lwsWindow" aria-hidden="true">
<header class="lws-head"><div class="lws-brand"><img src="/Image/LWSGEN Logo.png" alt="Ladowebservis"><div><strong>Ladowebservis</strong><small>🟢 Nákupný poradca</small></div></div><button class="lws-close" id="lwsClose">×</button></header>
<div class="lws-intro"><strong>👋 Ahoj!</strong><p>Pomôžem ti s výberom produktu, akciami, objednávkou aj reklamáciou.</p></div>
<div class="lws-actions">
<button data-a="poradca">🛍️ Výber produktu</button><button data-a="zlava">🎁 Akcie a zľavy</button>
<button data-a="doprava">🚚 Doprava</button><button data-a="reklamacia">🛠️ Chcem riešiť reklamáciu</button>
<button data-a="it">💻 IT služby</button><button data-a="kontakt">📞 Kontakt</button>
</div>
<div class="lws-msgs" id="lwsMsgs"></div>
<div class="lws-input"><input id="lwsInput" placeholder="Napíš, s čím pomôžeme..."><button id="lwsSend">➤</button></div>
<div class="lws-foot">Ladowebservis • Rýchla pomoc</div>
</section>`;
        document.body.appendChild(w);
        var top = document.createElement("button");
        top.id = "lwsBackTop"; top.type = "button"; top.title = "Späť hore"; top.setAttribute("aria-label", "Späť hore"); top.textContent = "↑";
        document.body.appendChild(top);
        addStyles();
        window.addEventListener("scroll", function () { top.classList.toggle("show", window.pageYOffset > 300); });
        top.onclick = function () { window.scrollTo({ top: 0, behavior: "smooth" }); };

        document.getElementById('lwsOpen').onclick = function () { document.getElementById('lwsWindow').classList.add('open'); document.getElementById('lwsInput').focus(); };
        document.getElementById('lwsClose').onclick = function () { document.getElementById('lwsWindow').classList.remove('open'); };

        document.querySelectorAll('#lwsLiveChat [data-a]').forEach(function (b) {
            b.onclick = function () { bot(autoResponses[this.getAttribute('data-a')]); };
        });

        function user(t) { var m = document.createElement('div'); m.className = 'lws-msg lws-user'; m.innerHTML = '<strong>👤 Vy:</strong><br>' + esc(t); document.getElementById('lwsMsgs').appendChild(m); scroll(); }
        function bot(t) { var m = document.createElement('div'); m.className = 'lws-msg lws-bot'; m.innerHTML = '<strong>🤖 Poradca:</strong><br>' + t; document.getElementById('lwsMsgs').appendChild(m); scroll(); }
        function scroll() { var x = document.getElementById('lwsMsgs'); x.scrollTop = x.scrollHeight; }

        bot('Ahoj! 👋 Som nákupný poradca Ladowebservis.<br>Vyber si tému alebo mi jednoducho napíš, čo hľadáš. Pomôžem aj s IT službami.');

        function send() {
            var i = document.getElementById('lwsInput'), t = i.value.trim();
            if (!t) return;
            user(t); i.value = '';
            setTimeout(function () { bot(response(t)); }, 300);
        }
        document.getElementById('lwsSend').onclick = send;
        document.getElementById('lwsInput').onkeydown = function (e) { if (e.key === 'Enter') { e.preventDefault(); send(); } };
    }

    document.addEventListener('DOMContentLoaded', create);
})();