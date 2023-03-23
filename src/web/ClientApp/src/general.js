import React from 'react';
import ReactDOM from 'react-dom';

import Contact from './comps/contact';

let contactContainer = document.getElementById('contact');
if (contactContainer) {
    
    ReactDOM.render(
        <Contact />,
        contactContainer);
}