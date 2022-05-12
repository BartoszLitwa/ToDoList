import React, { useEffect, useState } from 'react';
import { useLocation } from 'react-router-dom';
import PopupCreateTask from '../../modals/Task/PopupCreateTask';
import TaskCard from './TaskCard';
import { WithRouter } from '../../WithRouter';
import PopupErrors from '../../modals/PopupErrors';

function TodoList() { 
    const location = useLocation();
    const [modal, setModal] = useState(false);
    const [value, setValue] = useState(0); // only for refreshing the view
    const [loading, setLoading] = useState(true);
    const [taskList, setTaskList] = useState(location.state.taskList);
    const [tempSearchTaskList, setTempSearchTaskList] = useState(location.state.taskList.tasks);
    const [modalErrors, setModalErrors] = useState(false);
    const [errors, setErrors] = useState([]);
    const [searchValue, setSearchValue] = useState('');

    const refreshToken = async () => {
        const userAuth = JSON.parse(localStorage.getItem('authUser'));
        await callEndpoint('identity/refresh', 'POST', {
          Token: userAuth.token,
          RefreshToken: userAuth.refreshToken,
        }, (res) => {
            console.log(res);

            if(res.token !== null && res.token !== ""){
                localStorage.setItem('authUser', JSON.stringify({
                    token: res.token,
                    refreshToken: res.refreshToken,
                    isLoggedIn: res.success,
                  }));
            }
        });
      }

    const callEndpoint = async (endpoint, method, jsonObj, setState) => {
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
                await refreshToken();
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

            if(json.errors !== null){

            }

            await setState(json);
        } catch (err) {
            console.warn(err);
        }
    }

        // Load only once
    useEffect(() => {
        setLoading(true);
        async function fetchData() {
            await callEndpoint('taskList/get/' + taskList.id, 'GET', 
            null,
            async (res) => {
                setTaskList(res);
            });
        }; 
        fetchData();
        setLoading(false);
        // Disable warning for not passing the dependecy for callendpoint and tasklist.id
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    const addTask = async (task) => {
        await callEndpoint("task/create", "POST", 
        {
            TaskListId: taskList.id,
            DueDate: task.DueDate,
            CompletedDate: task.CompletedDate,
            Title: task.Title,
            Description: task.Description,
            IsCompleted: task.IsCompleted 
        },
        (res) => {
            let tempList = taskList;
            tempList.tasks.push(res);
            setTaskList(tempList);
            setTempSearchTaskList(tempList.tasks);
            setSearchValue('');
        });
    }

    const deleteTask = async (task) => {
        await callEndpoint("task/delete", "DELETE", 
        {
            TaskId: task.id,
        },
        (res) => {
            if(res.Success === true) {
                let tList = taskList;
                const oldIndex = taskList.tasks.findIndex((x) => x.id === task.id);

                // Remove 1 item
                tList.tasks.splice(oldIndex, 1);
                setTaskList(tList);
                setTempSearchTaskList(tList.tasks);
                setSearchValue('');
                setValue(value + 1);
            }
        });
    }

    const updateTask = async (task) => {
        await callEndpoint("task/update", "PUT", 
        {
            TaskId: task.id,
            DueDate: task.dueDate,
            CompletedDate: task.completedDate,
            Title: task.title,
            Description: task.description,
            IsCompleted: task.isCompleted 
        },
        (res) => {
            // find old index
            const oldIndex = taskList.tasks.findIndex((x) => x.id === task.id);

            let tList = taskList;
            // Remove previous
            tList.tasks = taskList.tasks.filter((tl) => { 
                return task.id !== tl.id;
            });

            // push new item at the same place
            tList.tasks.splice(oldIndex, 0, res);

            setTaskList(tList);
            setTempSearchTaskList(tList.tasks);
            setSearchValue('');
            setValue(value + 1);
        });
    }

    const toggleTask = async (task) => {
        console.log(task)
        await callEndpoint("task/toggle", "PATCH", 
        {
            TaskId: task.Id,
            completedDate: task.CompletedDate
        },
        (res) => {
            // find old index
            const oldIndex = taskList.tasks.findIndex((x) => x.id === task.Id);
            let tempList = taskList;
            tempList.tasks[oldIndex].isCompleted = res.isCompleted;
            tempList.tasks[oldIndex].completedDate = res.completedDate;

            setTaskList(tempList);
            setTempSearchTaskList(tempList.tasks);
            setSearchValue('');
            setValue(value + 1);
        });
    }

    const generateTaskCards = () => {
        if(taskList.tasks === undefined)
        {
            return;
        }

        return taskList.tasks
        .filter((x) => x.title.toLowerCase().includes(searchValue) || x.description.toLowerCase().includes(searchValue))
        .map((obj, index) => {
            return <TaskCard task={obj} index={index} key={obj.id} 
            addTask={addTask} deleteTask={deleteTask} updateTask={updateTask} toggleTask={toggleTask}/>
        });
    }

    const handleChange = (e) => {
        const {name, value} = e.target;
        if(name === "searchValue") {
            console.log(value.length)
            if(value.length > 0){
                let temp = taskList.tasks.filter((x) => {
                    return x.title.includes(value) || x.description.includes(value);
                });

                setTempSearchTaskList(temp);
                console.log(tempSearchTaskList)
            } else {
                setTempSearchTaskList(taskList.tasks);
                console.log(tempSearchTaskList)
            }
            setSearchValue(value.toLowerCase());
        }
    }

    const loadPageHandler = () => {
        if(loading === true){
            return <div className='center'>
                <h1>Loading...</h1>
            </div>
        } else {
            return <div>
                <div className='center' style={{"top": "110px", "position": "fixed", "backgroundColor": "white", "width": "95%"}}>
                    <div className='header-wrapper '>
                        <h3>{ '"' + taskList.title + '" Todo List'}</h3>
                        <div className="form-group" style={{'display': 'flex', 'flexDirection':'row'}}>
                            <label className='m-3'>Search: </label>
                            <input type="text" className="form-control" name="searchValue"
                             value={searchValue} onChange={handleChange}/>
                        </div>
                        <button className='btn btn-primary m-3'
                        onClick={() => setModal(true)}>Create Task</button>
                    </div>
                </div>
                <div className='task-container'>
                    { generateTaskCards() }
                </div>
            </div>         
        }
    }

    return (
        <header className=''>
            { loadPageHandler()}
            <PopupCreateTask task={taskList} modal={modal} toggle={() => setModal(!modal)} handleTask={addTask}/>
            <PopupErrors toggle = {() => setModalErrors(!modalErrors)} modal={modalErrors} errors={errors} clearData={() => setErrors([])} />
        </header>
    );
}
 
export default WithRouter(TodoList);