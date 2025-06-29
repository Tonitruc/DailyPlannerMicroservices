FROM rabbitmq:4.1-management

# Download and install the delayed message exchange plugin
RUN set -eux; \
    apt-get update; \
    apt-get install -y --no-install-recommends wget; \
    wget -O /plugins/rabbitmq_delayed_message_exchange-4.1.0.ez \
      https://github.com/rabbitmq/rabbitmq-delayed-message-exchange/releases/download/v4.1.0/rabbitmq_delayed_message_exchange-4.1.0.ez; \
    apt-get purge -y --auto-remove wget; \
    rm -rf /var/lib/apt/lists/*; \
    chown rabbitmq:rabbitmq /plugins/rabbitmq_delayed_message_exchange-4.1.0.ez; \
    chmod 644 /plugins/rabbitmq_delayed_message_exchange-4.1.0.ez; \
    rabbitmq-plugins enable --offline rabbitmq_delayed_message_exchange

EXPOSE 5672 15672

CMD ["rabbitmq-server"]