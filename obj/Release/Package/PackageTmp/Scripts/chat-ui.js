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

    // ===== CHAT FUNCTIONALITY =====
    const autoResponses = {
        'cena': '💰 <strong>Cenové informácie:</strong><br>Naše produkty majú konkurenčné ceny. Ceny nájdete na stránke Produkty. Pre zľavový balík: kód NOVYROK26.',
        'doprava': '🚚 <strong>Doprava:</strong><br>• Bezplatne nad 50€ v SR<br>• Doba: 3-7 pracovných dní<br>• Vývodný čas: Ut-Pia<br>• Poistená a sledovaná',
        'platba': '💳 <strong>Platba:</strong><br>Bezpečná platba cez Stripe. Všetky údaje sú šifrované a chránené.',
        'vratenie': '↩️ <strong>Vrátenie:</strong><br>• 120 dní<br>• Bezpodmienečne<br>• Bez skrytých poplatkov',
        'produkty': '🛍️ <strong>Produkty:</strong><br>✓ BalanceOil - Omega-3<br>✓ Zinobiotic - Probiotiká<br>✓ CollagenBoozt - Kolagén<br>✓ Vitamin D Test',
        'balanceoil': '⭐ <strong>BalanceOil:</strong><br>Prírodné Omega-3. Zdravé srdce a mozog. Dostupné v rôznych formách.',
        'zlava': '🎁 <strong>NOVYROK26:</strong><br>10% zľava na vybrané produkty',
        'kontakt': '📞 <strong>Kontakt:</strong><br>☎️ +421917952432<br>📧 info@ladowebservis.sk<br>Po-Pia 9:00-17:00',
        'pomoc': '❓ <strong>Ako pomôcť?</strong><br>💰 cena | 🚚 doprava | 💳 platba<br>🛍️ produkty | 🎁 zľava | 📞 kontakt'
    };

    function escapeHtml(text) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }

    function addMessage(text, sender = 'user') {
        const chatMessages = document.getElementById('chat-messages');
        if (!chatMessages) return;

        const messageDiv = document.createElement('div');
        messageDiv.style.marginBottom = '8px';

        if (sender === 'user') {
            messageDiv.innerHTML = '<div style="background: #5d33fb; color: #fff; border-radius: 6px; padding: 8px; margin-left: 20px; word-wrap: break-word;"><strong style="font-size:11px;">Ty:</strong> ' + escapeHtml(text) + '</div>';
        } else {
            messageDiv.innerHTML = '<div style="background: #e7f3ff; border-radius: 6px; padding: 8px; border-left: 4px solid #5d33fb; margin-right: 20px; font-size: 11px; line-height: 1.4;"><strong style="color: #5d33fb;">Support:</strong> ' + text + '</div>';
        }

        chatMessages.appendChild(messageDiv);
        chatMessages.scrollTop = chatMessages.scrollHeight;
    }

    function getAutoResponse(input) {
        const query = input.toLowerCase().trim();

        // Direct match
        if (autoResponses[query]) {
            return autoResponses[query];
        }

        // Partial match
        for (const key in autoResponses) {
            if (query.includes(key)) {
                return autoResponses[key];
            }
        }

        // Default response
        return 'Ďakujem za otázku! Môžem ti pomôcť s: 💰 cenou, 🚚 dopravou, 💳 platbou, 🛍️ produktami, 🎁 zľavou alebo 📞 kontaktom.';
    }

    function sendMessage() {
        const chatInput = document.getElementById('chat-input');
        if (!chatInput) return;

        const message = chatInput.value.trim();
        if (!message) return;

        addMessage(message, 'user');
        chatInput.value = '';
        chatInput.focus();

        // Simulate bot typing delay
        setTimeout(function () {
            const response = getAutoResponse(message);
            addMessage(response, 'bot');
        }, 400);
    }

    // ===== RENDER CHAT UI =====
    function initializeChatUI() {
        const container = document.getElementById('side-cart-chat-container');
        if (!container) return;

        // Build chat HTML dynamically
        container.className = 'side-cart-chat';
        container.innerHTML = `
            <h5 style="margin-top: 15px; margin-bottom: 10px; color: #5d33fb;">💬 Podpora</h5>

            <!-- Chat Messages Container -->
            <div id="chat-messages" style="background: #f9f9f9; border: 1px solid #e0e0e0; border-radius: 6px; padding: 8px; height: 200px; overflow-y: auto; margin-bottom: 8px; font-size: 12px;">
                <div style="text-align: center; color: #999; padding: 10px;">
                    Ahoj! 👋 Ako ti môžem pomôcť?
                </div>
            </div>

            <!-- Chat Input Group -->
            <div class="chat-input-group" style="display: flex; gap: 6px; margin-bottom: 10px;">
                <input id="chat-input"
                       type="text"
                       class="form-control"
                       placeholder="Napíš otázku..."
                       style="font-size: 12px; padding: 6px;"
                       aria-label="Chat message input" />
                <button id="chat-send-btn"
                        class="btn btn-sm btn-primary"
                        style="font-size: 12px; padding: 6px 10px; white-space: nowrap;"
                        aria-label="Send message">
                    <i class="fa fa-send" aria-hidden="true"></i>
                </button>
            </div>

            <!-- Quick Help Section -->
            <div style="background: #e7f3ff; padding: 8px; border-radius: 4px; border-left: 3px solid #5d33fb; font-size: 11px; color: #333; line-height: 1.4;">
                <p style="margin: 0 0 6px 0;"><strong style="color: #5d33fb;">Skratky:</strong></p>
                <p style="margin: 2px 0;">💰 cena | 🚚 doprava | 💳 platba</p>
                <p style="margin: 2px 0;">🛍️ produkty | 🎁 zľava | 📞 kontakt</p>
            </div>
        `;

        // Attach event listeners after rendering
        const chatSendBtn = document.getElementById('chat-send-btn');
        const chatInput = document.getElementById('chat-input');

        if (chatSendBtn) {
            chatSendBtn.addEventListener('click', sendMessage);
        }

        if (chatInput) {
            chatInput.addEventListener('keypress', function (e) {
                if (e.key === 'Enter' && !e.shiftKey) {
                    e.preventDefault();
                    sendMessage();
                }
            });
        }
    }

    // Initialize when DOM is ready
    document.addEventListener('DOMContentLoaded', function () {
        initializeChatUI();
    });
})();
