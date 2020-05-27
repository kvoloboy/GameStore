const navLinks = document.querySelectorAll('.menu-link');
navLinks.forEach(l => {
    if (document.location.pathname === l.getAttribute('href')){
        l.classList.add('menu-link-orange')
    }
});
