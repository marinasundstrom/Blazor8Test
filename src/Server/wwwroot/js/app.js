export function init() {
    const offCanvasCollapse = document.querySelector('.offcanvas-collapse');

    // Toggle off canvas when clicking the menu toggle
    document.querySelector('#navbarSideCollapse').addEventListener('click', () => {
        offCanvasCollapse.classList.toggle('open');
    });

    // Toggle offcanvas off when clicking links
    document.querySelectorAll('a.nav-link')
        .forEach(linkElement => {
            if(linkElement.classList.contains("dropdown-toggle"))
            {
                // Toggle offcanvas when clicking the items, 
                // but ignore clicking the dropdown itself

                const linkElementParent = linkElement.parentElement;
                linkElementParent.querySelectorAll(".dropdown-item")
                    .forEach(itemElement => itemElement.addEventListener('click', () => {
                        offCanvasCollapse.classList.toggle('open');
                    }));
                return;
            }

            linkElement.addEventListener('click', () => {
                offCanvasCollapse.classList.toggle('open');
            }) 
        });
}

init();