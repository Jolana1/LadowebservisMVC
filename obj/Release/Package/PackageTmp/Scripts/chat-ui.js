/// <reference path="jquery-3.4.1.min.js" />
/// <reference path="jquery.validate.min.js" />
/// <reference path="jquery.validate.unobtrusive.min.js" />

(function () {
    'use strict';

    // ===== GO TO TOP BUTTON FUNCTIONALITY =====
    const goTopBtn = document.getElementById('goTopBtn');

    // Show/hide button based on scroll position
    window.addEventListener('scroll', function () {
        if (window.pageYOffset > 300) {
            goTopBtn.classList.add('show');
        } else {
            goTopBtn.classList.remove('show');
        }
    });

    // Smooth scroll to top function
    window.scrollToTop = function () {
        const scrollDuration = 500; // milliseconds
        const scrollStep = window.pageYOffset / (scrollDuration / 15);

        const scrollInterval = setInterval(function () {
            if (window.pageYOffset > 0) {
                window.scrollBy(0, -scrollStep);
            } else {
                clearInterval(scrollInterval);
            }
        }, 15);
    };

    // ===== IMPROVED CHAT FUNCTIONALITY =====
    const autoResponses = {
        'cena': '💰 <strong>Cenové informácie:</strong><br>Naše produkty majú konkurenčné ceny. Všetky ceny nájdete na stránke Produkty. Skúste kód NOVYROK26 pre zľavu!',
        'doprava': '🚚 <strong>Doprava:</strong><br>• ✓ Bezplatne nad 50€ v SR<br>• ✓ Doba: 3-7 pracovných dní<br>• ✓ Poistená a sledovaná',
        'platba': '💳 <strong>Bezpečná Platba:</strong><br>Používame Stripe - bezpečnú platobnú bránu. Všetky vaše údaje sú šifrované.',
        'vratenie': '↩️ <strong>Vrátenie Tovarov:</strong><br>• ✓ 120 dní na vrátenie<br>• ✓ Bez otázok<br>• ✓ Bezplatne',
        'produkty': '🛍️ <strong>Naša Ponuka:</strong><br>BalanceOil | Zinobiotic | CollagenBoozt | Vitamin D Test',
        'balanceoil': '⭐ <strong>BalanceOil:</strong><br>Prírodný Omega-3 olej. Podporuje srdce, mozog a zrak. Dostupný: 300ml.',
        'zlava': '🎁 <strong>Zľava NOVYROK26:</strong><br>10% zľava na produkty nad 50€. Použite pri checkout!',
        'kontakt': '📞 <strong>Kontaktujte Nás:</strong><br>☎️ +421917952432<br>📧 info@ladowebservis.sk<br>⏰ Po-Pia: 9:00-17:00',
        'pomoc': '❓ <strong>Ako ti pomôžem?</strong><br>Napíš: cena, doprava, platba, produkty, zľava, vratenie alebo kontakt',
        'bonusové body': '⭐ <strong>Bonusové Body:</strong><br>Za každých €10 nákupu dostaneš 1 bod. 100 bodov = €10 zľava!',
        'program': '🎯 <strong>Vernostný Program:</strong><br>Zbieraj body, získavaj zľavy a exkluzívne ponuky!'
    };

    // Generate products grid for side cart - REMOVED
    function renderSideCartProducts() {
        // Products section has been removed from side cart
        return;
    }

    // Add product to cart - REMOVED
    window.addProductToCart = function(productId) {
        // Products section has been removed from side cart
        return;
    };

    // Send chat message
    window.sendChatMessage = function() {
        const chatInput = document.getElementById('chat-input');
        const chatMessages = document.getElementById('chat-messages');

        if (!chatInput || !chatMessages) return;

        const message = chatInput.value.trim();
        if (!message) return;

        // Add user message
        const userDiv = document.createElement('div');
        userDiv.className = 'chat-message user';
        userDiv.innerHTML = '<strong>👤 Vy:</strong> ' + escapeHtml(message);
        chatMessages.appendChild(userDiv);

        chatInput.value = '';
        chatInput.focus();
        chatMessages.scrollTop = chatMessages.scrollHeight;

        // Simulate bot typing
        setTimeout(function() {
            const response = getAutoResponse(message);
            const botDiv = document.createElement('div');
            botDiv.className = 'chat-message bot';
            botDiv.innerHTML = '<strong>🤖 Support:</strong> ' + response;
            chatMessages.appendChild(botDiv);
            chatMessages.scrollTop = chatMessages.scrollHeight;
        }, 300);
    };

    function escapeHtml(text) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }

    function getAutoResponse(input) {
        const query = input.toLowerCase().trim();

        // Direct match
        if (autoResponses[query]) {
            return autoResponses[query];
        }

        // Partial match - check each keyword
        for (const key in autoResponses) {
            if (query.includes(key)) {
                return autoResponses[key];
            }
        }

        // Czech/Slovak aliases
        if (query.includes('peniaze') || query.includes('penize')) return autoResponses['cena'];
        if (query.includes('vzorka') || query.includes('vzorky')) return autoResponses['produkty'];
        if (query.includes('porada')) return autoResponses['pomoc'];

        // Default response
        return '😊 Pomôžem ti rád! Napíš niečo z: <strong>cena, doprava, platba, produkty, zľava, vratenie, kontakt</strong>';
    }

    // Initialize chat when DOM is ready
    document.addEventListener('DOMContentLoaded', function() {
        // Products section has been removed - no need to render

        // Add enter key support to chat input
        const chatInput = document.getElementById('chat-input');
        if (chatInput) {
            chatInput.addEventListener('keypress', function(e) {
                if (e.key === 'Enter' && !e.shiftKey) {
                    e.preventDefault();
                    sendChatMessage();
                }
            });
        }

        // Display welcome message
        const chatMessages = document.getElementById('chat-messages');
        if (chatMessages && chatMessages.children.length === 0) {
            const welcomeDiv = document.createElement('div');
            welcomeDiv.className = 'chat-message bot';
            welcomeDiv.innerHTML = '<strong>🤖 Support:</strong> Ahoj! 👋 Ako ti môžem pomôcť? Napíš <strong>pomoc</strong> pre ponuku otázok.';
            chatMessages.appendChild(welcomeDiv);
        }
    });

})();
