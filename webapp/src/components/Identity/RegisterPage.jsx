import React, { Component } from 'react';
import { Link } from "react-router-dom";
import PopupErrors from '../../modals/PopupErrors';
import { WithRouter } from '../../WithRouter';

class RegisterPage extends Component {
    constructor(props) {
        super(props);
        this.state = {
            modal: false,
            errors: [],
            username: "",
            password: "",
            email: "",
            firstName: "",
            lastName: ""
        }
    }

    render() { 
        const handleLogin = async () => {
            let taskObj = {};
            taskObj["Username"] = this.state.username;
            taskObj["Password"] = this.state.password;
            taskObj["Email"] = this.state.email;
            taskObj["FirstName"] = this.state.firstName;
            taskObj["LastName"] = this.state.lastName;

            await this.props.callEndpoint("identity/register", "POST", 
            taskObj,
            async (res) => {
                if(res.success === true){
                    this.props.userAuth.token = res.token;
                    this.props.userAuth.refreshToken = res.refreshToken;
                    this.props.userAuth.isLoggedIn = true;
                    localStorage.setItem('authUser', JSON.stringify(this.props.userAuth));
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
            } else if(name === "email") {
                this.setState({email: value});
            } else if(name === "firstName") {
                this.setState({firstName: value});
            } else if(name === "lastName") {
                this.setState({lastName: value});
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
                    <h1>Register Page</h1>
                    <div className='text-center'>
                        <form>
                            <div className="form-group">
                                <label>Username</label>
                                <input type="text" className="form-control" name="username"
                                    value={this.state.username} onChange={handleChange}/>
                            </div>
                            <div className="form-group">
                                <label>Password</label>
                                <input type="password" rows="5" className='form-control' name="password"
                                value={this.state.password} onChange={handleChange}/>
                            </div>
                            <div className="form-group">
                                <label>Email</label>
                                <input type="text" className="form-control" name="email"
                                    value={this.state.email} onChange={handleChange}/>
                            </div>
                            <div className="form-group">
                                <label>First Name</label>
                                <input type="text" rows="5" className='form-control' name="firstName"
                                value={this.state.firstName} onChange={handleChange}/>
                            </div>
                            <div className="form-group">
                                <label>Last Name</label>
                                <input type="text" rows="5" className='form-control' name="lastName"
                                value={this.state.lastName} onChange={handleChange}/>
                            </div>
                        </form>

                        <div className='m-3'>
                            <button className='btn btn-primary m-3' onClick={async () => await handleLogin()}>Register</button>
                            <button className='btn btn-primary m-3'>
                                <Link to="/login" className='text-white'>Login Page</Link>
                            </button>
                        </div>
                    </div>

                    <PopupErrors toggle = {toggle} modal={this.state.modal} errors={this.state.errors} clearData={clearData} />
                </header>
            </div>
         );
    }
}
 
export default WithRouter(RegisterPage);