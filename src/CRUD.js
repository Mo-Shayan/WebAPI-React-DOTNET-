import React, {useState, useEffect, Fragment} from "react";
import Table from 'react-bootstrap/Table';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Container from 'react-bootstrap/Container';
import axios from "axios";
import Form from 'react-bootstrap/Form';
import InputGroup from 'react-bootstrap/InputGroup';
import Dropdown from 'react-bootstrap/Dropdown';
import DropdownButton from 'react-bootstrap/DropdownButton';
const CRUD = () =>{
    const proddata = [
        {
            ProductId : 1,
            Name : "Boohoman hoodie",
            Price : 1500,
            Color : "Black",
            type : "pullover"
        },
        {
            ProductId : 2,
            Name : "Boohoman hoodie",
            Price : 1500,
            Color : "white",
            type : "pullover"
        },
        {
            ProductId : 3,
            Name : "Boohoman hoodie",
            Price : 1500,
            Color : "Black",
            type : "zipper"
        }
    ]
    const [data,setData] = useState([]);
    useEffect(()=>{
    getData();
},[])

const getData = () => {
    axios.get('https://localhost:7174/api/Product')
    .then((result)=>{
        setData(result.data)
    })
    .catch((error)=>{
        console.log(error);
    })

}
const handleSort = (order) => {
    // Clone the current data array to avoid mutating the state directly
    const sortedData = [...data];

    // Perform sorting based on the price field
    sortedData.sort((a, b) => {
      const priceA = a.price;
      const priceB = b.price;

      if (order === 'asc') {
        return priceA - priceB;
      } else {
        return priceB - priceA;
      }
    });

    setData(sortedData);
  };


const [show, setShow] = useState(false);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);


  const [name,setName] = useState('')
  const [price,setPrice] = useState('')
  const [color,setColor] = useState('')
  const [type,setType] = useState('')
 
  const [editID,setEditID] = useState('')
  const [editname,setEditName] = useState('')
  const [editprice,setEditPrice] = useState('')
  const [editcolor,setEditColor] = useState('')
  const [edittype,setEditType] = useState('')
    const handleEdit = (id) => {
        handleShow();
        axios.get(`https://localhost:7174/api/Product/${id}`)
        .then((res)=>{
           setEditName(res.data.name);
           setEditPrice(res.data.price);
           setEditType(res.data.type);
           setEditColor(res.data.color);
           setEditID(id);
        })
        .catch((error)=>
        {
            console.log(error);
        })
    }
    const handleDelete = (id) => {
        if(window.confirm("Are you sure you want to delete this product?")== true)
        {
            axios.delete('https://localhost:7174/api/Product/' + id)
            .then((result)=>{
                if(result.status === 200)
                {
                    console.log("delete success");
                    getData();
                }
            })
            .catch((error)=>
            {
                console.log(error);
            })
           
        }
        
      }


      const handleUpdate = () =>{
        const url = `https://localhost:7174/api/Product/${editID}`;
        const data = {
            "productID": editID,
            "name": editname,
            "price": editprice,
            "color": editcolor,
            "type": edittype
        }
        axios.put(url, data)
        .then((result) => {
            handleClose();
              getData();
              clear();
        }).catch((error)=>{
          console.log(error);
        })
        // console.log(editID+editname+editcolor+editprice+edittype);
      }

      const handleSave= () =>{
        const url = 'https://localhost:7174/api/Product';
        const data = {
            
            "name": name,
            "price": price,
            "color": color,
            "type": type
          }
          axios.post(url, data)
          .then((result) => {
                getData();
                clear();
          }).catch((error)=>{
            console.log(error);
          })
        }
  
        const clear = () => {
            setName('');
            setPrice(0);
            setColor('');
            setType('');
            setEditName('');
            setEditPrice(0);
            setEditColor('');
            setEditType('');
            setEditID('');
        }
        const [search,setSearch] = useState('');
        console.log(search);

       
    return (
        
        <Fragment>
            
        <br/>
             <h2>Add New Product</h2>
              <Container>
                <Row>
                    <Col>
                    <input type="text" className="form-control" placeholder="Enter Name" 
                    value={name} onChange={(e) => setName(e.target.value)}
                    />
                    </Col>
                    <Col>
                    <input type="text" className="form-control" placeholder="Enter Price"
                    value={price} onChange={(e) => setPrice(e.target.value)}/>
                    </Col>
                    <Col>
                    <input type="text" className="form-control" placeholder="Enter Color" 
                    value={color} onChange={(e) => setColor(e.target.value)}/>
                    </Col>
                    <Col>
                    <input type="text" className="form-control" placeholder="Enter Type" 
                    value={type} onChange={(e) => setType(e.target.value)}/>
                    </Col>
                    <Col >
                    <button className="btn btn-primary" onClick={()=> handleSave()}>Submit</button>
                    </Col>
                </Row>
              
                </Container>
                <br></br>
                <Form>
                    <InputGroup className="my-3" style={{ width: '50%' }} >
                        <Form.Control onChange={(e)=>{setSearch(e.target.value)}} placeholder="Search Products"/>
                    </InputGroup>
                </Form>
                <br/>
                <DropdownButton id="dropdown-basic-button" title="Apply Filters" style={{ width: '0%', marginTop: '-5rem', marginLeft:'80%'}} >
                <Dropdown.Item onClick={() => handleSort("asc")} >Price-Low to High</Dropdown.Item>
                <Dropdown.Item onClick={() => handleSort("desc")}>Price-High to Low</Dropdown.Item>
                
                </DropdownButton>
                <br />
         <Table striped bordered hover>
            <thead>
                <tr>
                <th>#</th>
                <th>Product ID</th>
                <th>Name</th>
                <th>Price</th>
                <th>Color</th>
                <th>Type</th>
                <th>Actions</th>

                </tr>
            </thead>
            <tbody>
                {
                    data && data.length> 0 ?
                    data
                    .filter((item) => {
                        return search.toLowerCase() === ''?item : item.name.toLowerCase().includes(search)
                    })
                    .map((item, index)=>{
                        return(
                            <tr key={index}>
                            <td>{index+ 1}</td>
                            <td>{item.productID}</td>
                            <td>{item.name}</td>
                            <td>{item.price}</td>
                            <td>{item.color}</td>
                            <td>{item.type}</td>
                            <td colSpan={2}>
                                <button className="btn btn-primary" onClick={()=> handleEdit(item.productID)}>Edit</button> &nbsp;
                                <button className="btn btn-danger" onClick={()=> handleDelete(item.productID)}>Delete</button>

                            </td>
                            </tr>
                        )
                    })
                    :
                    'Loading..'
                }
                
               
            </tbody>
         </Table>

         <Modal show={show} onHide={handleClose}>
        <Modal.Header closeButton>
          <Modal.Title>Modify product</Modal.Title>
        </Modal.Header>
        <Modal.Body>
        <Col>
       
                    <Col>
                     Name
                    <input type="text" className="form-control" placeholder="Enter Name" 
                    value={editname} onChange={(e) => setEditName(e.target.value)}
                    />
                    </Col>
                    <br/>
                    <Col>
                    Price
                    <input type="text" className="form-control" placeholder="Enter Price"
                    value={editprice} onChange={(e) => setEditPrice(e.target.value)}/>
                    </Col>
                    <br/>
                    <Col>
                    Color
                    <input type="text" className="form-control" placeholder="Enter Color" 
                    value={editcolor} onChange={(e) => setEditColor(e.target.value)}/>
                    </Col>
                    <br/>
                    <Col>
                    Type
                    <input type="text" className="form-control" placeholder="Enter Type" 
                    value={edittype} onChange={(e) => setEditType(e.target.value)}/>
                    </Col>
                    <br/>
                    
                </Col>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleClose}>
            Close
          </Button>
          <Button variant="primary" onClick={handleUpdate}>
            Save Changes
          </Button>
        </Modal.Footer>
      </Modal>
        </Fragment>
    )
}
export default CRUD;