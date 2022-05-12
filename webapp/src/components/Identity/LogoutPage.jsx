import React, { Component } from 'react';
import PopupErrors from '../../modals/PopupErrors';
import { WithRouter } from '../../WithRouter';

class LogoutPage extends Component {
    constructor(props) {
        super(props);
        this.state = {
            modal: false,
            success: false,
            errors: null
        }
    }

    render() { 
        const toggle = () => {
            this.setState({modal: !this.state.modal});
        }

        const clearData = () => {
            this.setState({errors: []});
        }

        const logout = async () => {
            await this.props.callEndpoint("identity/logout", "GET", 
            null,
            async (res) => {
                console.log(res)
                if(res.success === true){
                    localStorage.setItem('authUser', JSON.stringify({
                        token: "",
                        refreshToken: "",
                        isLoggedIn: false,
                      }));

                      this.props.navigate('/home');
                      window.location.reload(false);
                } else {
                    this.setState({errors: res.errors, modal: true});
                }
            });

        }

        const stayLoggedIn = () => {
            this.props.navigate('/todotasklist');
        }

        return (  
            <div className='content'>
                <div className='center logout-wrapper'>
                    <h1 style={{"color": "red"}}>Are you sure You want to log out?</h1>
                    <div className='m-20' style={{"display": "flex", "direction": "row", "justifyContent": "space-evenly"}}>
                        <button className='btn btn-primary' style={{"backgroundColor": "white", "color": "green", "fontSize": "20px", "fontWeight": "bolder"}}
                        onClick={async () => await logout()}>Yes</button>
                        <button className='btn btn-primary' style={{"backgroundColor": "white", "color": "red", "fontSize": "20px", "fontWeight": "bolder"}}
                        onClick={stayLoggedIn}>No</button>
                    </div>
                </div>

                <PopupErrors toggle = {toggle} modal={this.state.modal} errors={this.state.errors} clearData={clearData} />
            </div>
        );
    }
}
 
export default WithRouter(LogoutPage);