import React from 'react';
import {PageHeader} from 'react-bootstrap';
import {Image} from 'react-bootstrap';

const Header = () => {
    return (       
        <div >            
            <PageHeader ><Image src="../images/ODTLogo.png" rounded /><small style={alignLeft}>Welcome Cea ! <a href="/account/logoff" >Logout</a></small></PageHeader>  
        </div>
  );
};

const alignLeft = {float : 'right'};

export default Header