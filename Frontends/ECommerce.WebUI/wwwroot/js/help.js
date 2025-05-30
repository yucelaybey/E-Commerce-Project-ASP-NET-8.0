document.addEventListener('DOMContentLoaded', function() {
    // Mobile Menu Toggle
    const mobileMenuBtn = document.querySelector('.mobile-menu-btn');
    const mobileMenuClose = document.querySelector('.mobile-menu-close');
    const mobileMenu = document.querySelector('.mobile-menu');
    const mobileMenuOverlay = document.querySelector('.mobile-menu-overlay');
    
    mobileMenuBtn.addEventListener('click', function() {
        mobileMenu.classList.add('active');
        mobileMenuOverlay.classList.add('active');
        document.body.style.overflow = 'hidden';
    });
    
    mobileMenuClose.addEventListener('click', function() {
        mobileMenu.classList.remove('active');
        mobileMenuOverlay.classList.remove('active');
        document.body.style.overflow = '';
    });
    
    mobileMenuOverlay.addEventListener('click', function() {
        mobileMenu.classList.remove('active');
        mobileMenuOverlay.classList.remove('active');
        document.body.style.overflow = '';
    });
    
    // Category Cards Click Event
    const categoryCards = document.querySelectorAll('.category-card');
    const faqSections = document.querySelectorAll('.faq-section');
    
    categoryCards.forEach(card => {
        card.addEventListener('click', function() {
            const category = this.getAttribute('data-category');
            
            // Hide all FAQ sections
            faqSections.forEach(section => {
                section.classList.remove('active');
            });
            
            // Show selected FAQ section
            const targetSection = document.getElementById(`${category}-faq`);
            if (targetSection) {
                targetSection.classList.add('active');
                
                // Scroll to the section
                targetSection.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });
    
    // FAQ Accordion
    const faqQuestions = document.querySelectorAll('.faq-question');
    
    faqQuestions.forEach(question => {
        question.addEventListener('click', function() {
            this.classList.toggle('active');
            const answer = this.nextElementSibling;
            answer.classList.toggle('active');
            
            // Close other open answers
            if (this.classList.contains('active')) {
                faqQuestions.forEach(q => {
                    if (q !== this && q.classList.contains('active')) {
                        q.classList.remove('active');
                        q.nextElementSibling.classList.remove('active');
                    }
                });
            }
        });
    });
    
    // Search Functionality
    const helpSearch = document.getElementById('helpSearch');
    const searchButton = document.getElementById('searchButton');
    const searchResults = document.getElementById('searchResults');
    
    // FAQ data for search
    const faqData = [];
    
    document.querySelectorAll('.faq-item').forEach(item => {
        const question = item.querySelector('.faq-question h3').textContent;
        const answer = item.querySelector('.faq-answer p').textContent;
        const section = item.closest('.faq-section').id;
        
        faqData.push({
            question,
            answer: answer.substring(0, 100) + '...',
            section
        });
    });
    
    function performSearch(query) {
        if (query.length < 3) {
            searchResults.innerHTML = '';
            searchResults.classList.remove('active');
            return;
        }
        
        const results = faqData.filter(item => 
            item.question.toLowerCase().includes(query.toLowerCase()) || 
            item.answer.toLowerCase().includes(query.toLowerCase())
        );
        
        displaySearchResults(results);
    }
    
    function displaySearchResults(results) {
        searchResults.innerHTML = '';
        
        if (results.length === 0) {
            const noResult = document.createElement('div');
            noResult.className = 'search-result-item';
            noResult.textContent = 'Sonuç bulunamadı';
            searchResults.appendChild(noResult);
        } else {
            results.forEach(result => {
                const resultItem = document.createElement('div');
                resultItem.className = 'search-result-item';
                resultItem.innerHTML = `
                    <h4>${result.question}</h4>
                    <p>${result.answer}</p>
                `;
                
                resultItem.addEventListener('click', function() {
                    // Show the relevant FAQ section
                    faqSections.forEach(section => {
                        section.classList.remove('active');
                    });
                    
                    const targetSection = document.getElementById(result.section);
                    if (targetSection) {
                        targetSection.classList.add('active');
                        
                        // Open the specific FAQ item
                        const faqItems = targetSection.querySelectorAll('.faq-item');
                        faqItems.forEach(item => {
                            const itemQuestion = item.querySelector('.faq-question h3').textContent;
                            if (itemQuestion === result.question) {
                                item.querySelector('.faq-question').classList.add('active');
                                item.querySelector('.faq-answer').classList.add('active');
                                
                                // Scroll to the question
                                item.scrollIntoView({
                                    behavior: 'smooth',
                                    block: 'center'
                                });
                            } else {
                                item.querySelector('.faq-question').classList.remove('active');
                                item.querySelector('.faq-answer').classList.remove('active');
                            }
                        });
                        
                        // Scroll to the section
                        targetSection.scrollIntoView({
                            behavior: 'smooth',
                            block: 'start'
                        });
                    }
                    
                    // Clear search
                    helpSearch.value = '';
                    searchResults.classList.remove('active');
                });
                
                searchResults.appendChild(resultItem);
            });
        }
        
        searchResults.classList.add('active');
    }
    
    // Search events
    helpSearch.addEventListener('input', function() {
        performSearch(this.value);
    });
    
    searchButton.addEventListener('click', function() {
        performSearch(helpSearch.value);
    });
    
    // Close search results when clicking outside
    document.addEventListener('click', function(e) {
        if (!helpSearch.contains(e.target) && !searchResults.contains(e.target) && !searchButton.contains(e.target)) {
            searchResults.classList.remove('active');
        }
    });
    
    // Show first FAQ section by default
    if (faqSections.length > 0) {
        faqSections[0].classList.add('active');
    }
});