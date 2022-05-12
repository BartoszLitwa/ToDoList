import React, { Component } from 'react';

class AccountPage extends Component {
    constructor(props) {
        super(props);
        this.state = {

        }
    }



    render() { 
        return (  
            <div>
                <div className='center' style={{"top": "110px", "position": "fixed", "backgroundColor": "white", "width": "95%"}}>
                    <div className='header-wrapper '>
                        <h3>{ "'s account" }</h3>
                        <button className='btn btn-primary m-3'
                        onClick={() => {}}>Account</button>
                    </div>
                </div>
            </div>
        );
    }
}
 
export default AccountPage;