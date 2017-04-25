import React from 'react';
import { PageHeader } from 'react-bootstrap';
import { Image } from 'react-bootstrap';

const Header = () => {
    return (
        <div >
                <Image src="../images/ODTLogo.png" rounded />
                <div style={alignLeft} >
                   <h4> Welcome Cea! <a href="/account/logoff" >Logout</a></h4>
                </div>
            <hr/>
        </div>
    );
};

const alignLeft = { float: 'right' };

export default Header