import React, { Component } from 'react';
import './App.css';
import TodoList from './components/Task/TodoList';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Route, Link, Routes } from "react-router-dom";
import LoginPage from './components/Identity/LoginPage';
import RegisterPage from './components/Identity/RegisterPage';
import LogoutPage from './components/Identity/LogoutPage';
import TodoTaskList from './components/TaskList/TodoTaskList';
import HomePage from './components/home/HomePage';
import { WithRouter } from './WithRouter';
import AccountPage from './components/Account/AccountPage';

class App extends Component {
  constructor(props) {
    super(props);

    this.state = {
      userAuth: {
        token: "",
        refreshToken: "",
        isLoggedIn: false,
      },
      //counter: 1,
    }

    this.callEndpoint = this.callEndpoint.bind(this);
  }

  async componentDidMount() {
    await this.loadFromLocalStorageUserAuth();

    await this.refreshToken()
    this.interval = setInterval(async () => await this.refreshToken(), 1 * 60 * 1000); // Mins * seconds * ms
  }

  componentWillUnmount() {
    // clear the interval at the end
    clearInterval(this.interval);
  }

  refreshPage() {
    //this.setState({counter: this.state.counter + 1});
  }

  async loadFromLocalStorageUserAuth(){
    const auth = JSON.parse(localStorage.getItem('authUser'));
    if(auth !== null) {
      this.state.userAuth = auth;
    } else {
      localStorage.setItem('authUser', JSON.stringify({
        token: "",
        refreshToken: "",
        isLoggedIn: false,
      }));
    }

    if(!auth.isLoggedIn){
      this.props.navigate('/home');
      return;
    }

    return await this.callEndpoint('account/get', 'GET', null, async (res) => {
        // Unathorized
        if(res.status === 401){
          await this.refreshToken();
      }
    })
  }

  async refreshToken() {
    const auth = JSON.parse(localStorage.getItem('authUser'));
    if(auth.isLoggedIn !== true){
      return;
    }

    await this.callEndpoint('identity/refresh', 'POST', {
      Token: auth.token,
      RefreshToken: auth.refreshToken,
    }, (res) => {
      if(res.token !== null && res.token !== null){
        localStorage.setItem('authUser', JSON.stringify({
            token: res.token,
            refreshToken: res.refreshToken,
            isLoggedIn: res.success,
          }));
      } else if(res.errors !== null) {
        localStorage.setItem('authUser', JSON.stringify({
          token: "",
          refreshToken: "",
          isLoggedIn: false,
        }));
        this.props.navigate('/');
      }
    });
  }

  async callEndpoint(endpoint, method, jsonObj, setState) {
    const baseUrl = "https://localhost:5001/api/v1/";
    const userAuth = JSON.parse(localStorage.getItem('authUser'));
    const token = userAuth === undefined  ? '' : userAuth.token;
    try {
      const response = await fetch(baseUrl + endpoint, {
        "method": method,
        headers: {
          "Authorization": "bearer " + token,
          "Access-Control-Allow-Origin": "*",
          "content-type": "application/json",
          "accept": "application/json",
          "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
        },
        body: method.toLowerCase() === 'get' ? null : JSON.stringify(jsonObj)
      });
      // Unathorized
      if(response.status === 401){
        await this.refreshToken();
        return;
      }

      // No Content - delete successful
      if(response.status === 204){
        await setState( {
          Success: true
        });
        return;
      }

      const json = await response.json();
      await setState(json);
      this.setState({});
    } catch (err) {
      console.warn(err);
    }
  }

  render() {
    const renderLoggedIn = () => {
      if(this.state.userAuth.isLoggedIn === true){
        return <div>
          <button className='btn btn-primary m-3' >
            <Link to="/todotasklist" className='text-white'>Task Lists</Link>
          </button>
          <button className='btn btn-primary m-3' >
            <Link to="/account" className='text-white'>Account</Link>
          </button>
        </div>
      }
    }

    const renderLogoutButton = () => {
      if(this.state.userAuth.isLoggedIn === true){
        return <button className='btn btn-primary m-3'>
                <Link to="/logout" className='text-white'>Logout</Link>
              </button>
      }
    }

    const renderLoginRegister = () => {
      if(this.state.userAuth.isLoggedIn !== true){
        return <div>
                  <button className='btn btn-primary m-3'>
                    <Link to="/login" className='text-white'>Login Page</Link>
                  </button>

                  <button className='btn btn-primary m-3'>
                    <Link to="/register" className='text-white'>Register Page</Link>
                  </button>
                </div>
      }
    }

    return (
      <div className="App">
          <div className='center' style={{"top": "0", "position": "fixed", "width": "95%", "backgroundColor": "white"}}>
            <div className='header-wrapper'>
              <button className='btn btn-primary' style={{"backgroundColor": "white"}}>
                <Link to="/home" className='text-black m-10' style={{"fontSize": "40px"}}>ToDo App</Link>
              </button>
              { renderLoggedIn() }

              <div>
                {renderLoginRegister()}

                {renderLogoutButton()}
              </div>
            </div>
          </div>

          <div className='App-intro text-center mt-3' >
              <Routes>
                <Route path="/home" element={<HomePage callEndpoint={this.callEndpoint} userAuth={this.state.userAuth} refreshPage={this.refreshPage} />} />
                <Route path="/todolist" element={<TodoList callEndpoint={this.callEndpoint} userAuth={this.state.userAuth} refreshPage={this.refreshPage} />} />
                <Route path="/todotasklist" element={<TodoTaskList callEndpoint={this.callEndpoint} userAuth={this.state.userAuth} refreshPage={this.refreshPage} />} />
                <Route path="/login" element={<LoginPage callEndpoint={this.callEndpoint} userAuth={this.state.userAuth} refreshPage={this.refreshPage} />} />
                <Route path="/register" element={<RegisterPage callEndpoint={this.callEndpoint} userAuth={this.state.userAuth} refreshPage={this.refreshPage} />} />
                <Route path="/logout" element={<LogoutPage callEndpoint={this.callEndpoint} userAuth={this.state.userAuth} refreshPage={this.refreshPage} />} />
                <Route path="/account" element={<AccountPage callEndpoint={this.callEndpoint} userAuth={this.state.userAuth} refreshPage={this.refreshPage} />} />
              </Routes>
          </div>
      </div>
    );
  }
}

export default WithRouter(App);