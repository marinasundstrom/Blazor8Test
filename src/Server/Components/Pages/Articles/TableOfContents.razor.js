export function init() {
    // simple function to use for callback in the intersection observer
    const changeNav = (entries, observer) => {
        // if(!entries[0].isIntersecting) return;

        entries.map(entry => {
            if (entry.isIntersecting) {
                // remove old active class
                document.querySelectorAll('.active').forEach(el => el.classList.remove('active'));

                // get id of the intersecting section
                var id = entry.target.getAttribute('id');

                // find matching link & add appropriate class
                document.querySelector(`[href$="#${id}"]`).classList.add("active");
            }
        })
    }

    // init the observer
    const options = {
        rootMargin: '0px',
        threshold: 1.0
    }

    const observer = new IntersectionObserver(changeNav, options);

    // target the elements to be observed
    const sections = document.querySelectorAll('section');
    sections.forEach((section) => {
        observer.observe(section);
    });
}