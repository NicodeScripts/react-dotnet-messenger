import { useState } from "react";
import { InputGroup, Button, Form } from "react-bootstrap";

const SendMessageForm = ({ sendMessage }) => {
    const [msg, setMessage] = useState('');  // Correct state initialization

    return (
        <Form onSubmit={e => {
            e.preventDefault();
            sendMessage(msg);
            setMessage('');  // Clear the input after sending the message
        }}>
            <InputGroup className="mb-3">
                <InputGroup.Text>Chat</InputGroup.Text>
                <Form.Control
                    onChange={e => setMessage(e.target.value)}  // Update state correctly
                    value={msg}
                    placeholder="Type a message"
                />
                <Button variant="primary" type="submit" disabled={!msg}>
                    Send
                </Button>
            </InputGroup>
        </Form>
    );
};

export default SendMessageForm;