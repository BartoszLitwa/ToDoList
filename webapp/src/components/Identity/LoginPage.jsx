import React from 'react';
import { Link } from "react-router-dom";
import PopupErrors from '../../modals/PopupErrors';
import {WithRouter} from '../../WithRouter';

class LoginPage extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            username: "",
            password: "",
            modal: false,
            errors: null
        }
    }

    render() { 
        const handleLogin = async () => {
            let taskObj = {};
            taskObj["Username"] = this.state.username;
            taskObj["Password"] = this.state.password;

            await this.props.callEndpoint("identity/login", "POST", 
            taskObj,
            async (res) => {
                if(res.success === true){
                    this.props.userAuth.token = res.token;
                    this.props.userAuth.refreshToken = res.refreshToken;
                    this.props.userAuth.isLoggedIn = true;
                    localStorage.setItem("authUser", JSON.stringify(this.props.userAuth));
                    this.props.refreshPage();
                    this.props.navigate('/todotasklist');
                } else {
                    this.setState({errors: res.errors, modal: true});
                }
            });
        }
    
        const handleChange = (e) => {
            const {name, value} = e.target;
            if(name === "username") {
                this.setState({username: value});
            } else if(name === "password") {
                this.setState({password: value});
            }
        }

        const toggle = () => {
            this.setState({modal: !this.state.modal});
        }

        const clearData = () => {
            this.setState({errors: []});
        }

        return ( 
            <div className='center content'>
                <header className='form-wrapper'>
                    <h1>Login Page</h1>
                    <div className='text-center'>
                        <form>
                            <div className="form-group width-200">
                                <label>Username</label>
                                <input type="text" className="form-control" name="username"
                                    value={this.state.username} onChange={handleChange}/>
                            </div>
                            <div className="form-group">
                                <label>Password</label>
                                <input type="password" rows="5" className='form-control' name="password"
                                value={this.state.password} onChange={handleChange}/>
                            </div>
                        </form>

                        <div className='m-3'>
                            <button className='btn btn-primary m-3' onClick={async () => await handleLogin()}>Login</button>
                            <button className='btn btn-primary m-3'>
                                <Link to="/register" className='text-white'>Register Page</Link>
                            </button>
                        </div>
                    </div>

                    <PopupErrors toggle = {toggle} modal={this.state.modal} errors={this.state.errors} clearData={clearData} />
                </header>
            </div>
         );
    }
}
 
export default WithRouter(LoginPage);