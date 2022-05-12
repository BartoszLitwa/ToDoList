import React, { Component } from 'react';
import PopupCreateTaskList from '../../modals/TaskList/PopupCreateTaskList';
import TaskListCard from './TaskListCard';

class TodoTaskList extends Component {
    constructor(props){
        super(props);
        this.state = {
            modal:false,
            taskList: [],
            tempSearchTaskList: [],
            searchValue: '',
            counter: 1
        };
    }

    componentDidMount(){
        this.props.callEndpoint("taskList/get", "GET", 
            this.props.taskListObj,
            (res) => {
                this.setState({taskList: res, tempSearchTaskList: res});
            });
    }

    update() {
        this.setState({counter: this.state.counter + 1});
    }

    render() { 
        const toggle = () => {
            this.setState({modal: !this.state.modal});
        }

        const saveTaskList = (taskObj) => {
            let tempList = this.state.taskList;
            tempList.push(taskObj);
            this.setState({taskList: tempList, modal: false});
        }
        
        const setTaskListObj = (taskobj, index) => {
            let temp = this.state.taskList;
            temp[index] = taskobj;
            this.setState({taskList: temp})
        }

        const deleteTaskListObj = (taskListObj) => {
            this.setState({taskList: this.state.taskList.filter((tl) => { 
                return taskListObj.id !== tl.id;
            })});
        }

        const updateTaskListObj = (taskListObj) => {
            // find old index
            const oldIndex = this.state.taskList.findIndex((x) => x.id === taskListObj.id );

            let tempList = this.state.taskList;
            // Remove previous
            tempList = this.state.taskList.filter((tl) => { 
                return taskListObj.id !== tl.id;
            });

            taskListObj.tasks = this.state.taskList[oldIndex].tasks;
            // push new item at the same place
            tempList.splice(oldIndex, 0, taskListObj);

            this.setState({taskList: tempList});
        }

        const handleChange = (e) => {
            const {name, value} = e.target;
            if(name === "searchValue") {
                this.setState({searchValue: value});
            }
        }

        const generateTaskListCards = () => {
            if(this.state.taskList === undefined)
            {
                return;
            }

            return this.state.taskList
            .filter((x) => x.title.toLowerCase().includes(this.state.searchValue) || x.description.toLowerCase().includes(this.state.searchValue))
            .map((obj, index) => {
                return <TaskListCard taskListObj = {obj} index = {index} key={obj.id} callEndpoint={this.props.callEndpoint}
                 setTaskListObj={setTaskListObj} deleteTaskListObj={deleteTaskListObj} updateTaskListObj={updateTaskListObj}/>
            });
        }

        return (
            <header className=''>
                <div className='center' style={{"top": "110px", "position": "fixed", "backgroundColor": "white", "width": "95%"}}>
                    <div className='header-wrapper '>
                        <h3>Your TaskLists</h3>

                        <div className="form-group" style={{'display': 'flex', 'flexDirection':'row'}}>
                            <label className='m-3'>Search: </label>
                            <input type="text" className="form-control" name="searchValue"
                             value={this.state.searchValue} onChange={handleChange}/>
                        </div>

                        <div>
                            <select>
                            <label className='m-3'>Search: </label>
                             <option value="volvo">Volvo</option>
                                <option value="saab">Saab</option>
                                <option value="mercedes">Mercedes</option>
                                <option value="audi">Audi</option>
                             </select>
                        </div>

                        <button className='btn btn-primary' style={{"height": "50px"}}
                        onClick={() => this.setState({modal: true})}>Create Task List</button>
                    </div>
                </div>
                <div className='task-container'>
                    { generateTaskListCards() }
                </div>

                <PopupCreateTaskList toggle = {toggle} modal={this.state.modal} handleTask={saveTaskList} callEndpoint={this.props.callEndpoint}/>
            </ header>
        );
    }
}
 
export default TodoTaskList;